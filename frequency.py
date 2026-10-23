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

print("--------------------------------:Reading CSV as dictionary:----------------------------------------")
with open("companies.csv", "r", newline="", encoding="utf-8") as file:
    reader = csv.DictReader(file)

    for row in reader:
        print(row)        