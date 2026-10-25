import pandas as pd
import numpy as np


arr1 = np.arange(0,2)
arr2 = np.arange(6,10).reshape(2,-1)

arr3 = np.array([[0,1],[1,0]])
result_arr = arr1 + arr2
print(result_arr)
concat_horizontally = np.concatenate((arr2, arr3),axis=1) # horizontal concatenation
concat_vertically = np.concatenate((arr2, arr3),axis=0) # vertical concatenation
print(concat_vertically) # print concatenation along axis 1
print(concat_horizontally) # print concatenation along axis 0
result_tem = np.argsort([4,1,5,7,0])
print(result_tem)
print(result_tem[3:5])
print(result_tem[-2:])


# Define the dictionary containing the data
data = {
    "Name": ["John", "Emma", "Luke", "Sophia", "Oliver"],
    "Age": [25, 30, 22, 28, 35],
    "Gender": ["Male", "Female", "Male", "Female", "Male"],
    "Salary": [50000, 60000, 45000, 70000, 80000]
}

# Create the pandas DataFrame
df = pd.DataFrame(data)
df.set_index("Name", inplace=True)
df.reset_index("Name",inplace=True)
# Display the DataFrame
print(df)
print(df.shape)
result = df.loc[(df['Age'] > 25) & (df["Age"]<=30)] 
print(result)

result_one = df[(df["Age"]>25) & (df["Age"]<=30)] 
print(result_one)

result_salary_gender = df.groupby("Gender").aggregate(max_salary=("Salary", "max"),median_salary=("Salary", "median"))
print(result_salary_gender)
# We use axis=1 because it tells pandas to apply the function row by row instead of column by column.
# 
# 1. With axis=1 (Row-by-Row) ➡️

# Pandas feeds the rows into the lambda function one by one as a Series where the index labels are the column headers.
# • First loop (x is John's data): Looks at x["Age"] (25) and x["Salary"] (50000). Calculates bonus.
# • Second loop (x is Emma's data): Looks at x["Age"] (28) and x["Salary"] (60000). Calculates bonus.
# • This is exactly what you want when comparing multiple columns for a single record.
df["bonus"] = df[["Salary", "Age"]].apply(lambda x: x["Salary"] * 0.1 if x["Age"] > 30 else x["Salary"] * 0.15, axis=1)
print(df)


# Original Series
s = pd.Series([10, 20, 30], index=['a', 'b', 'c'])

# Create a deep copy
s_deep = s.copy(deep=True)
f_deep = s.copy(deep=False)
# Modify the copy
s_deep['a'] = 999

print(s['a'])      # Output: 10  (Original remains unchanged)
print(s_deep['a']) # Output: 999 (Copy is modified)
print("Original Series after modifying deep copy:")
print(s)
print("Shallow copy:")
print(f_deep)
f_deep['a'] = 888
s_deep['a'] = 777
print("Shallow copy after modification:")
print(f_deep)
print("Original Series after modifying shallow copy:")
print(s)
# How to create empty data frame 
empty_df = pd.DataFrame(columns=["Name", "Age", "Gender", "Salary"])
print(empty_df)