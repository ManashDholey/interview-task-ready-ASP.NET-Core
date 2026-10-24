import os
import logging
import requests
import pandas as pd
from datetime import datetime
from sqlalchemy import create_engine, exc

# -------------------------------------------------------------------------
# SETUP & CONFIGURATION
# -------------------------------------------------------------------------
logging.basicConfig(
    level=logging.INFO, 
    format="%(asctime)s [%(levelname)s] %(message)s"
)

# API Configuration
API_URL = "https://example.com"
API_HEADERS = {"Authorization": "Bearer YOUR_API_TOKEN_HERE"}

# SQL Server Configuration (Using Windows Authentication / Trusted Connection)
# Swap UID/PWD into the connection string if using SQL Server Authentication
DB_SERVER = "YOUR_SERVER_NAME"
DB_NAME = "SalesDB"
TABLE_NAME = "stg_sales_data"
CONN_STR = f"mssql+pyodbc://{DB_SERVER}/{DB_NAME}?driver=ODBC+Driver+17+for+SQL+Server&trusted_connection=yes"

# -------------------------------------------------------------------------
# ETL FUNCTIONS
# -------------------------------------------------------------------------
def extract_sales_data(url: str, headers: dict) -> list:
    """Fetches raw sales data from the API endpoint."""
    logging.info("Starting extraction phase...")
    try:
        response = requests.get(url, headers=headers, timeout=30)
        response.raise_for_status() # Raises an exception for 4xx/5xx status codes
        
        data = response.json()
        # Adjust key based on your actual API response structure (e.g., data['records'])
        records = data.get("sales", data) 
        
        logging.info(f"Successfully extracted {len(records)} raw records.")
        return records
    except requests.exceptions.RequestException as e:
        logging.error(f"API Extraction failed: {e}")
        raise

def transform_and_validate(raw_data: list) -> pd.DataFrame:
    """Cleans, standardizes, and validates the extracted records."""
    logging.info("Starting transformation and validation phase...")
    
    if not raw_data:
        logging.warning("No data provided for transformation.")
        return pd.DataFrame()

    # 1. Load into DataFrame
    df = pd.DataFrame(raw_data)

    # 2. Deduplication (Keep the most recent transaction if 'sale_date' exists)
    initial_count = len(df)
    if 'sale_date' in df.columns:
        df = df.sort_values('sale_date')
    df = df.drop_duplicates(subset=['transaction_id'], keep='last')
    dedup_count = initial_count - len(df)
    if dedup_count > 0:
        logging.info(f"Removed {dedup_count} duplicate records based on 'transaction_id'.")

    # 3. Data Type Standardization & Validation Constraints
    try:
        # Ensure mandatory columns exist
        required_cols = ['transaction_id', 'sale_date', 'amount', 'quantity']
        for col in required_cols:
            if col not in df.columns:
                raise ValueError(f"Missing critical column: {col}")

        # Drop rows where critical IDs or metrics are missing
        df = df.dropna(subset=['transaction_id', 'amount'])

        # Cast data types safely
        df['sale_date'] = pd.to_datetime(df['sale_date'])
        df['amount'] = pd.to_numeric(df['amount'], errors='coerce')
        df['quantity'] = pd.to_numeric(df['quantity'], errors='coerce').fillna(0).astype(int)

        # Business Rule Validation: Amounts and quantities must be positive numbers
        valid_rows = (df['amount'] > 0) & (df['quantity'] >= 0)
        invalid_count = len(df) - valid_rows.sum()
        
        if invalid_count > 0:
            logging.warning(f"Filtering out {invalid_count} records failing business rule validation (negative/null amounts).")
            df = df[valid_rows]

    except Exception as e:
        logging.error(f"Data validation/transformation failed: {e}")
        raise

    logging.info(f"Transformation complete. {len(df)} records verified and ready for load.")
    return df

def load_to_sql_server(df: pd.DataFrame, connection_string: str, table: str):
    """Loads the clean DataFrame into the target SQL Server table."""
    logging.info(f"Starting load phase into table '{table}'...")
    if df.empty:
        logging.warning("DataFrame is empty. Skipping load phase.")
        return

    try:
        # Create SQLAlchemy engine
        # fast_executemany=True dramatically speeds up bulk inserts in SQL Server via pyodbc
        engine = create_engine(connection_string, fast_executemany=True)
        
        # Append data to the table. 
        # Use if_exists='replace' if you want to overwrite the table structure completely each run.
        df.to_sql(
            name=table, 
            con=engine, 
            if_exists='append', 
            index=False, 
            chunksize=1000
        )
        logging.info("Load phase completed successfully.")
        
    except exc.SQLAlchemyError as e:
        logging.error(f"Database Load failed: {e}")
        raise
    finally:
        engine.dispose()
        print("Database connection disposed.")
# -------------------------------------------------------------------------
# PIPELINE EXECUTION
# -------------------------------------------------------------------------
if __name__ == "__main__":
    start_time = datetime.now()
    logging.info("Pipeline execution triggered.")
    
    try:
        # Step 1: Extract
        raw_records = extract_sales_data(API_URL, API_HEADERS)
        
        # Step 2: Transform & Validate
        clean_df = transform_and_validate(raw_records)
        
        # Step 3: Load
        load_to_sql_server(clean_df, CONN_STR, TABLE_NAME)
        
        duration = datetime.now() - start_time
        logging.info(f"Pipeline executed successfully in {duration.total_seconds():.2f} seconds.")
        
    except Exception as e:
        logging.critical(f"Pipeline failed: {e}")
    finally:
        logging.info("Pipeline execution finished.")
