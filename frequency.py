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