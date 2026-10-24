# What are API pagination, rate limits, and retry mechanisms?
import time
import random
import requests
from requests.exceptions import RequestException

# Configuration parameters
BASE_URL = "https://jsonplaceholder.typicode.com/posts"
PAGE_SIZE = 10         # Number of items per page (Pagination)
MAX_RETRIES = 5        # Max number of retries for transient errors (Retry Mechanism)
INITIAL_BACKOFF = 1.0  # Initial wait time in seconds for exponential backoff

def fetch_posts_page(page: int, per_page: int) -> list:
    """
    Fetches a single page of posts using pagination, rate limit parsing,
    and a robust retry mechanism with exponential backoff and jitter.
    """
    # 1. API Pagination query parameters
    params = {
        "_page": page,
        "_limit": per_page
    }
    
    backoff = INITIAL_BACKOFF
    
    for attempt in range(1, MAX_RETRIES + 1):
        try:
            print(f"Fetching page {page} (Attempt {attempt}/{MAX_RETRIES})...")
            response = requests.get(BASE_URL, params=params, timeout=10)
            
            # 2. Handle Rate Limits (HTTP 429 Too Many Requests)
            if response.status_code == 429:
                # Respect standard server headers if available, otherwise use backoff
                retry_after = response.headers.get("Retry-After")
                wait_time = int(retry_after) if retry_after and retry_after.isdigit() else backoff
                
                print(f"⚠️ Rate limited (429). Retrying after {wait_time}s...")
                time.sleep(wait_time)
                
                # Progress exponential delay counter for the next loop
                backoff *= 2 
                continue
                
            # If server throws a 5xx error, retry it. If 4xx (other than 429), raise immediately.
            response.raise_for_status()
            
            # Return successfully fetched data
            return response.json()
            
        except (RequestException, requests.exceptions.HTTPError) as e:
            # Check if it's the last attempt
            if attempt == MAX_RETRIES:
                print(f"❌ Failed to fetch page {page} after {MAX_RETRIES} attempts.")
                raise e
            
            # 3. Retry Mechanism: Exponential Backoff + Jitter
            # Randomize to prevent "thundering herd" problem if multiple clients hit the API
            jitter = random.uniform(0, 0.5)
            sleep_duration = backoff + jitter
            
            print(f"🔄 Request failed due to: {e}. Retrying in {sleep_duration:.2f}s...")
            time.sleep(sleep_duration)
            backoff *= 2  # Double the backoff duration for the next iteration

    return []

def main():
    all_posts = []
    current_page = 1
    
    print("🚀 Starting data sync script...")
    
    while True:
        try:
            # Loop through pages sequentially (Pagination)
            records = fetch_posts_page(page=current_page, per_page=PAGE_SIZE)
            
            # JSONPlaceholder returns an empty list [] when you request a page past the end
            if not records:
                print("🏁 Reached the end of the dataset.")
                break
                
            all_posts.extend(records)
            print(f"✅ Successfully fetched {len(records)} items from page {current_page}.")
            
            current_page += 1
            
            # Optional polite delay between successful page requests to prevent triggering rate limits
            time.sleep(0.2) 
            
        except Exception as err:
            print(f"💥 Pipeline halted due to an unrecoverable error: {err}")
            break

    print(f"\n📊 Data Sync Completed. Total posts downloaded: {len(all_posts)}")

if __name__ == "__main__":
    main()
