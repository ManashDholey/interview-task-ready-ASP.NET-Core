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
