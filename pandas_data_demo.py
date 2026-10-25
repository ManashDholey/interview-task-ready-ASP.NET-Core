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

# Display the DataFrame
print(df)
