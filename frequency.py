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

numbers = [1, 2, 3, 2, 4, 1, 5,4,6,9,21]
dupli = {}
for num in numbers:
    dupli[num] = dupli.get(num,0) +1
print(dupli)    
duplicates = {num: count for num, count in dupli.items() if count > 1}
print(duplicates)
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

