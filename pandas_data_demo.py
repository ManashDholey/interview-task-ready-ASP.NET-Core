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
a_one = np.array([1, 2, 3])
print(a_one.ndim) # to know the array dimension
print(a_one.shape) # to know the array shape
print(a_one.dtype) # to know the array data type
z1 = np.zeros(5)
print(z1) # print the array of zeros
z2 = np.zeros((2, 3)) 
print(z2) # print the 2x3 array of zeros
z3 = np.zeros([5,5])
print(z3) # print the 5x5 array of zeros

c = np.zeros([5,5],complex)
print(c) # print the 5x5 array of complex zeros
print(c.dtype) # print the data type of the complex array

p = np.ones([5,2,3],int)
print(p) # print the 5x2x3 array of ones
print("Shape:",p.shape) # print the shape of the 5x2x3 array of ones
print("Data type:",p.dtype) # print the data type of the 5x2x3 array of ones
print("Number of dimensions:",p.ndim) # print the number of dimensions of the 5x2x3 array of ones
print("Total size:",p.size) # print the total number of elements in the 5x2x3 array of ones
print("Data type:",p.dtype) # print the 5x2x3 array of ones again for verification
print("Item size:",p.itemsize) # print the size in bytes of each element in the 5x2x3 array of ones
print("Total size in bytes:", p.nbytes) # print the total size in bytes of the 5x2x3 array of ones
print("Transposed array:", p.transpose()) # print the transposed 5x2x3 array of ones
print("Flattened iterator:", p.flat) # print the flattened iterator of the 5x2x3 array of ones
print("Flattened array as list:", list(p.flat)) # print the flattened array as a list for easier visualization

# recycable and reusable array example
data = [['0', 'Y', 'N'], ['1', 'Y', 'Y'], ['2', 'N', 'Y'], ['3', 'Y', 'Y'], ['4', 'N', 'N']]
products = pd.DataFrame(data, columns=['product_id', 'low_fats', 'recyclable']).astype({'product_id':'int64', 'low_fats':'category', 'recyclable':'category'})
print(products) # print the products DataFrame with appropriate data types
def find_products(products: pd.DataFrame) -> pd.DataFrame:
    df = products
    fc = (df['low_fats'] == 'Y') & (df['recyclable'] == 'Y')
    df = df[fc]
    return df[['product_id']]

# Find products that are both low fats and recyclable
filtered_products = find_products(products)
print("Filtered products:")
print(filtered_products)

# Find Customer Referee
data = [[1, 'Will', None], [2, 'Jane', None], [3, 'Alex', 2], [4, 'Bill', None], [5, 'Zack', 1], [6, 'Mark', 2]]
customer = pd.DataFrame(data, columns=['id', 'name', 'referee_id']).astype({'id':'Int64', 'name':'object', 'referee_id':'Int64'})
print(customer) # print the customer DataFrame with appropriate data types

def find_customer_referee(customer: pd.DataFrame) -> pd.DataFrame:
    df = customer
    fc = (df['referee_id'] != 2 ) | (df['referee_id'].isnull())
    df = df[fc]
    return df[['id', 'name', 'referee_id']]

# Find customers who have a referee
filtered_customers = find_customer_referee(customer)
print("Filtered customers:")
print(filtered_customers)

data = [['Afghanistan', 'Asia', 652230, 25500100, 20343000000], ['Albania', 'Europe', 28748, 2831741, 12960000000], ['Algeria', 'Africa', 2381741, 37100000, 188681000000], ['Andorra', 'Europe', 468, 78115, 3712000000], ['Angola', 'Africa', 1246700, 20609294, 100990000000]]
world = pd.DataFrame(data, columns=['name', 'continent', 'area', 'population', 'gdp']).astype({'name':'object', 'continent':'object', 'area':'Int64', 'population':'Int64', 'gdp':'Int64'})
print(world) # print the world DataFrame with appropriate data types

def big_countries(world: pd.DataFrame) -> pd.DataFrame:
    df = world
    fc = (df["area"] >= 3000000) | (df['population']>= 25000000)
    df = df[fc]
    return df[["name","population","area"]]

# Find big countries
filtered_countries = big_countries(world)
print("Filtered countries:")
print(filtered_countries)

data = [[1, 3, 5, '2019-08-01'], [1, 3, 6, '2019-08-02'], [2, 7, 7, '2019-08-01'], [2, 7, 6, '2019-08-02'], [4, 7, 1, '2019-07-22'], [3, 4, 4, '2019-07-21'], [3, 4, 4, '2019-07-21']]
views = pd.DataFrame(data, columns=['article_id', 'author_id', 'viewer_id', 'view_date']).astype({'article_id':'Int64', 'author_id':'Int64', 'viewer_id':'Int64', 'view_date':'datetime64[ns]'})
print(views) # print the views DataFrame with appropriate data types


def article_views(views: pd.DataFrame) -> pd.DataFrame:
    df = views[views['author_id'] == views['viewer_id']]

    result = (
        df[['author_id']]
        .drop_duplicates()
        .rename(columns={'author_id': 'id'})
        .sort_values(by='id')
        .reset_index(drop=True)
    )

    return result
    

# Find article views where the viewer is not the author
filtered_views = article_views(views)
print("Filtered views:")
print(filtered_views)
data = [[1, 'Let us Code'], [2, 'More than fifteen chars are here!']]
tweets = pd.DataFrame(data, columns=['tweet_id', 'content']).astype({'tweet_id':'Int64', 'content':'object'})
print(tweets) # print the tweets DataFrame with appropriate data types
def invalid_tweets(tweets: pd.DataFrame) -> pd.DataFrame:
    df = tweets
    fc = df['content'].str.len() > 15
    df = df[fc]
    return df[['tweet_id']] 
# Find invalid tweets
filtered_tweets = invalid_tweets(tweets)
print("Filtered tweets:")
print(filtered_tweets)
data = [[1, 'Alice'], [7, 'Bob'], [11, 'Meir'], [90, 'Winston'], [3, 'Jonathan']]
employees = pd.DataFrame(data, columns=['id', 'name']).astype({'id':'int64', 'name':'object'})
data = [[3, 1], [11, 2], [90, 3]]
employee_uni = pd.DataFrame(data, columns=['id', 'unique_id']).astype({'id':'int64', 'unique_id':'int64'})
print(employees) # print the employees DataFrame with appropriate data types
print(employee_uni) # print the employee_uni DataFrame with appropriate data types

def replace_employee_id(employees: pd.DataFrame, employee_uni: pd.DataFrame) -> pd.DataFrame:
    df_emp = employees
    df_employee_uni = employee_uni
    df = pd.merge(left= df_emp, right = df_employee_uni ,how='left', on='id')
    return df[['unique_id','name']]  
# Replace employee IDs with unique IDs
replaced_employees = replace_employee_id(employees, employee_uni)
print("Replaced employees:")
print(replaced_employees)
data = [[1, 100, 2008, 10, 5000], [2, 100, 2009, 12, 5000], [7, 200, 2011, 15, 9000]]
sales = pd.DataFrame(data, columns=['sale_id', 'product_id', 'year', 'quantity', 'price']).astype({'sale_id':'Int64', 'product_id':'Int64', 'year':'Int64', 'quantity':'Int64', 'price':'Int64'})
data = [[100, 'Nokia'], [200, 'Apple'], [300, 'Samsung']]
product = pd.DataFrame(data, columns=['product_id', 'product_name']).astype({'product_id':'Int64', 'product_name':'object'})
print(sales) # print the sales DataFrame with appropriate data types
print(product) # print the product DataFrame with appropriate data types

def sales_analysis(sales: pd.DataFrame, product: pd.DataFrame) -> pd.DataFrame:
    df_sales = sales
    df_product =  product
    df = pd.merge(left = df_sales, right = df_product, how = 'inner', on='product_id')
    return df[['product_name','year','price']]
# Perform sales analysis
analysis_result = sales_analysis(sales, product)
print("Sales analysis result:")
print(analysis_result)
data = [[1, 23], [2, 9], [4, 30], [5, 54], [6, 96], [7, 54], [8, 54]]
visits = pd.DataFrame(data, columns=['visit_id', 'customer_id']).astype({'visit_id':'Int64', 'customer_id':'Int64'})
data = [[2, 5, 310], [3, 5, 300], [9, 5, 200], [12, 1, 910], [13, 2, 970]]
transactions = pd.DataFrame(data, columns=['transaction_id', 'visit_id', 'amount']).astype({'transaction_id':'Int64', 'visit_id':'Int64', 'amount':'Int64'})
print(visits) # print the visits DataFrame with appropriate data types
print(transactions) # print the transactions DataFrame with appropriate data types

def find_customers(visits: pd.DataFrame, transactions: pd.DataFrame) -> pd.DataFrame:
    df_visits = visits
    df_tran = transactions
    df_tran = transactions
    df = pd.merge(left = df_visits , right= df_tran, how='left',on='visit_id')
    fc = df['transaction_id'].isnull()
    return df[fc].groupby('customer_id').agg({'visit_id':'count'}).reset_index().rename(columns={'visit_id':'count_no_trans'})
# Find customers with no transactions
customers_no_trans = find_customers(visits, transactions)
print("Customers with no transactions:")
print(customers_no_trans)
