name = "Manash Dholey"
freq = {}
for char in name:
    freq[char] = freq.get(char, 0) + 1
print(freq)

numbers = [1, 2, 3, 2, 4, 1, 2]

frequency = {}

for number in numbers:
    frequency[number] = frequency.get(number, 0) + 1

print(frequency)

def power(base, exponent):
    result = 1

    for _ in range(abs(exponent)):
        result *= base

    if exponent < 0:
        if base == 0:
            raise ValueError("Zero cannot have a negative exponent")
        return 1 / result

    return result


print(power(2, 5))    # 32
print(power(2, -3))   # 0.125
print(power(5, 0))    # 1
#Count the frequency of each element in a list.
numbers = [1, 2, 3, 2, 4, 1, 5,4,6,9,21]
dupli = {}
for num in numbers:
    dupli[num] = dupli.get(num,0) +1
print(dupli)    
duplicates = {num: count for num, count in dupli.items() if count > 1}
print(duplicates)
#Find the second-largest distinct number in a list without sorting it.
max_number = numbers[0]
secomd_max = 0
for num in numbers:
    if max_number < num:
        secomd_max = max_number
        max_number = num
    elif secomd_max < num and num != max_number:
        secomd_max = num

print("Max number:", max_number)
print("Second max number:", secomd_max)
# Remove duplicates from a list while preserving the original order.
unique_list = list(dict.fromkeys(numbers))
print(unique_list)

seen = []
unique_numbers = []
for num in numbers:
    if num not in seen:
        seen.append(num)
        unique_numbers.append(num)
#print(seen)
print(unique_numbers)
# Given two dictionaries, merge them and add the values when keys overlap.
a = {"sales": 100, "marketing": 200}
b = {"sales": 150, "finance": 300}
merged = a.copy()
for key, value in b.items():
    merged[key] = merged.get(key, 0) + value

print(merged)
#Reverse a string without using slicing ([::-1]).
string = "Manash Dholey"
reversed_string = ""
for char in string:
    #print(char)
    reversed_string = char + reversed_string
    #print(reversed_string)
print(reversed_string)
plan = "racecar"
#Check whether a string is a palindrome, ignoring spaces and letter case.
cleaned_string = "".join(char.lower() for char in plan if char.isalnum())
is_palindrome = cleaned_string == cleaned_string[::-1]
print(is_palindrome)
#Count the occurrences of each character in a string.
char_count_string = "occurrences of each"
char_count = {}
for char in char_count_string:
    char_count[char] = char_count.get(char, 0) + 1
print(char_count)
# Find the first non-repeating character in a string.
first_non_repeating = None
string_to_check = "occurrences of each"
char_frequency = {}
for char in string_to_check:
    char_frequency[char] = char_frequency.get(char, 0) + 1
for char in string_to_check:
    if char_frequency[char] == 1:
        first_non_repeating = char
        break

print(first_non_repeating)

#Write a function to check whether a number is prime.
def is_prime(n):
    if n <= 1:
        return False
    for i in range(2, int(n ** 0.5) + 1):
        if n % i == 0:
            return False
    return True

print(is_prime(2))   # True
print(is_prime(4))   # False
print(is_prime(17))  # True

#Write a function to calculate factorial using both iteration and recursion.
def factorial_iterative(n):
    if n < 0:
        return None
    result = 1
    for i in range(1, n + 1):
        result *= i
    return result

def factorial_recursive(n):
    if n < 0:
        return None
    if n == 0 or n == 1:
        return 1
    return n * factorial_recursive(n - 1)

print(factorial_iterative(5))  # 120
print(factorial_recursive(6))  # 720
#What is the difference between *args and **kwargs?
# *args allows a function to accept any number of positional arguments.
# **kwargs allows a function to accept any number of keyword arguments.

def example_function(*args, **kwargs):
    print("Positional arguments:", args)
    print("Keyword arguments:", kwargs)

example_function(1, 2, 3, a=4, b=5)

# Explain mutable and immutable objects in Python. What is the difference between a list and a tuple?
# Mutable objects can be changed after creation, while immutable objects cannot.
# Lists are mutable, meaning their elements can be modified.
# Tuples are immutable, meaning their elements cannot be modified after creation.   

#What is the difference between shallow copy and deep copy?
# A shallow copy creates a new object but does not create copies of nested objects; it references the original nested objects.
# A deep copy creates a new object and recursively copies all nested objects, creating independent copies.
# Explain try, except, else, and finally with an example
try:
    result = 10 / 2
except ZeroDivisionError:
    print("Error: Division by zero.")
else:
    print("Division successful. Result:", result)
finally:
    print("Execution completed.")
# How do you create and raise a custom exception?
# Define a custom exception class.
class CustomError(Exception):
    pass

# Raise the custom exception.
try:
    raise CustomError("This is a custom error message.")
except CustomError as e:
    print("Caught custom exception:", e)
# What is the difference between raise and return?    
# The raise statement is used to trigger an exception, which interrupts the normal flow of the program.
# The return statement is used to exit a function and optionally pass a value back to the caller.

def example_raise_return(x):
    if x < 0:
        raise ValueError("Negative value not allowed.")
    return x * 2

try:
    print(example_raise_return(5))   # 10
    print(example_raise_return(-3))  # Raises ValueError
except ValueError as e:
    print("Caught exception:", e)
# How would you handle invalid input while processing thousands of records without stopping the entire program?    
# You can handle invalid input by using try-except blocks within a loop that processes each record.
# This way, errors for individual records are caught and logged, while the loop continues processing the remaining records.

records = [10, 5, 0, -3, 8]
for record in records:
    try:
        if record < 0:
            raise ValueError("Negative value not allowed.")
        result = 100 / record
    except Exception as e:
        print(f"Error processing record {record}: {e}")
    else:
        print(f"Processed record {record}: Result = {result}")
# How would you handle invalid input while processing thousands of records without stopping the entire program?
# You can use a similar approach as above, with try-except blocks inside the loop to catch and log errors for individual records, allowing the loop to continue processing the remaining records.
# What is the difference between an iterator and a generator? Why is a generator useful for large datasets?
# An iterator is an object that implements the __iter__() and __next__() methods, allowing sequential access to elements.
# A generator is a special type of iterator created using a function with the yield statement.
# Generators are useful for large datasets because they produce items on-the-fly and do not store the entire dataset in memory.

def large_dataset_generator(n):
    for i in range(n):
        yield i

for item in large_dataset_generator(5):
    print(item)
# Write Python code to read a CSV file and print each row.
import csv

with open('companies.csv', newline='') as csvfile:
    reader = csv.reader(csvfile)
    for row in reader:
        print(row)
    csvfile.close()

print("--------------------------------:Reading CSV as dictionary:----------------------------------------")
with open("companies.csv", "r", newline="", encoding="utf-8") as file:
    reader = csv.DictReader(file)

    for row in reader:
        print(row)
    file.close()
print("--------------------------------:Reading JSON string as dictionary:----------------------------------------")
# How do you convert a JSON string into a Python dictionary?
# You can use the json.loads() function from the json module to parse a JSON string into a Python dictionary.

import json

json_string = '{"name": "Alice", "age": 30, "city": "New York"}'
data = json.loads(json_string)
print(data)
print("loads for dictionary:",type(data))  # Output: <class 'dict'>
# What is the difference between json.load() and json.loads()?
print("json.load() reads from a file object and parses the JSON content directly.")
print("json.loads() parses a JSON string into a Python dictionary.")
# Open the file and parse its contents directly
with open("logs.1787983461746.json", "r") as file:
    data = json.load(file)
    file.close()
print(data)
print("load method for list:",type(data))  # Output:  <class 'list'>

# Given the following JSON, extract the names of all employees whose salary exceeds 50,000.
print("--------------------------------:Extracting employees with salary > 50000:----------------------------------------")
data_json = {
    "employees": [
        {"name": "Amit", "salary": 45000},
        {"name": "Priya", "salary": 65000},
        {"name": "Rahul", "salary": 75000},
        {"name": "Priya3", "salary": 61000},
        {"name": "Rahul3", "salary": 72000},
        {"name": "Priya2", "salary": 68000},
        {"name": "Rahul2", "salary": 76000},
        {"name": "Priya1", "salary": 6000},
        {"name": "Rahul1", "salary": 7000}
    ]
}
print(data_json["employees"][0]["name"])
print(data_json["employees"][0]["salary"])
print(data_json["employees"][1])
print(data_json["employees"][2])
print(data_json["employees"][3])
data_list = data_json["employees"]
print(data_list)
print(type(data_list))
for employee in data_json["employees"]:
    if employee["salary"] > 50000:
        print(employee["name"])
#How would you handle missing keys or unexpected data types in an API response?
# You can use the get() method with a default value to handle missing keys.
# You can also use isinstance() to check the data type before processing.

for employee in data_json["employees"]:
    name = employee.get("name", "Unknown")
    salary = employee.get("salary", 0)
    if not isinstance(salary, (int, float)):
        continue
    if salary > 50000:
        print(name)
#  can you give the example of get method
example_dict = {"key1": "value1"}
print(example_dict.get("key1", "default_value"))  # Output: value1
print(example_dict.get("key2", "default_value"))  # Output: default_value

# How do you serialize a Python dictionary into a JSON file?
print("--------------------------------:Serializing dictionary into JSON file:----------------------------------------")
data_to_serialize = {
    "name": "Alice",
    "age": 30,
    "city": "New York"
}
with open("output.json", "w") as file:
    json.dump(data_to_serialize, file)
    file.close()
with open("output.json", "r") as file:
    data_loaded = json.load(file)
    file.close()
print(data_loaded)

#What is Pandas, and how does a DataFrame differ from a Series?
print("--------------------------------:Pandas DataFrame vs Series:----------------------------------------")
import pandas as pd

# Creating a DataFrame
data = {
    "Name": ["Alice", "Bob", "Charlie"],
    "Age": [25, 30, 35],
    "City": ["New York", "Los Angeles", "Chicago"]
}
df = pd.DataFrame(data)
print("DataFrame:")
print(df)

# Creating a Series
series = pd.Series([1, 2, 3, 4, 5])
print("Series:")
print(series)
print("Series from a list:")
# Creating a Series from a list
ages = pd.Series([25, 30, 35], name="Age")
print(ages)

# Read a CSV file into a DataFrame and display the first five rows.
print("--------------------------------:Reading CSV into DataFrame:----------------------------------------")
df_csv = pd.read_csv("companies.csv")
print(df_csv.head())

unique_df = df_csv.value_counts()
print("Unique value counts in the CSV DataFrame:")
print(unique_df)
# How do you find and remove duplicate rows in a DataFrame?
print("--------------------------------:Finding and Removing Duplicates:----------------------------------------")
duplicates = df_csv.duplicated()
print("Duplicate rows in the CSV DataFrame:")
print(duplicates)
print(len(duplicates))
df_csv_no_duplicates = df_csv.drop_duplicates()
print("CSV DataFrame after removing duplicate rows:")
print(df_csv_no_duplicates)

# How do you handle null values using isna(), dropna(), and fillna()?
print("--------------------------------:Handling Null Values:----------------------------------------")
print("Null values in the CSV DataFrame:")
print(df_csv.isna())

df_csv_no_nulls = df_csv.dropna()
print("CSV DataFrame after dropping rows with null values:")
print(df_csv_no_nulls)

df_csv_filled = df_csv.fillna("N/A")
print("CSV DataFrame after filling null values with 'N/A':")
print(df_csv_filled)
#Given a DataFrame containing CustomerId and Amount, calculate total sales for each customer.
df_sales = pd.read_csv("sales_one.csv")
print("Sales DataFrame:")
print(df_sales)
result = (
    df_sales.groupby("CustomerId", as_index=False)["Amount"]
      .sum()
      .rename(columns={"Amount": "TotalSales"})
)

print(result)
# Explain the difference between loc[] and iloc[].
print("--------------------------------:loc[] vs iloc[]:----------------------------------------")
print("Using loc[] to select rows by label:")
print(df_csv.loc[0])
print("Using iloc[] to select rows by integer position:")
print(df_csv.iloc[0])
print("Using loc[] to select a range of rows by label:")
print(df_csv.loc[0:1])
print("Using iloc[] to select a range of rows by integer position:")
print(df_csv.iloc[0:2])
print("Using loc[] to select specific columns by label:")
print(df_csv.loc[:, ["name", "careers_url"]])
print("Using iloc[] to select specific columns by integer position:")
print(df_csv.iloc[:, [0, 2]])
#How do you call a REST API using Python's requests library?
print("--------------------------------:Calling REST API:----------------------------------------")
import requests
url = "https://jsonplaceholder.typicode.com/posts/1"
response = requests.get(url)
print("Response from the REST API:")
print(response.json())
print("--------------------------------:Calling REST API by id:----------------------------------------")
url = "https://jsonplaceholder.typicode.com/posts/2"
response = requests.get(url)
print("Response from the REST API by id:")
print(response.json())
print("--------------------------------:Fatch All Posts:----------------------------------------")
url = "https://jsonplaceholder.typicode.com/posts"
response = requests.get(url)
print("Response from the REST API for all posts:")
print(response.json())
print("--------------------------------:Create a Post:----------------------------------------")
url = "https://jsonplaceholder.typicode.com/posts"
data = {
    "title": "foo",
    "body": "bar",
    "userId": 1
}
response = requests.post(url, json=data)
print("Response from the REST API for creating a post:")
print(response.json())
print("--------------------------------:Update a Post:----------------------------------------")
url = "https://jsonplaceholder.typicode.com/posts/1"
data = {
    "title": "updated title foo",
    "body": "updated body bar",
    "userId": 1
}
response = requests.put(url, json=data)
print("Response from the REST API for updating a post:")
print(response.json())
print("--------------------------------:Delete a Post:----------------------------------------")
url = "https://jsonplaceholder.typicode.com/posts/1"
response = requests.delete(url)
print("Response from the REST API for deleting a post:")
print(response.status_code)
print("--------------------------------:End of REST API Calls:----------------------------------------")