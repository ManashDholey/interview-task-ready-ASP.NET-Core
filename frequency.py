name = "Manash Dholey"
freq = {}
for char in name:
    freq[char] = freq.get(char, 0) + 1
print(freq)
