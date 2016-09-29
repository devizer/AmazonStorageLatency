## Amazon Storage Latency
Q: What is latency of SimpleDB, DynamoDB, Simple Storage, Simplae Queue and AIM?

|      Operation                       | Amazon, Same Region   | EE                     |
| :-------                             |                ---:   |                   ---: |
| DynamoDB, BatchWriteItem             |                   ms  |                229 ms  |
| DynamoDB, ListTables                 |                       |                233 ms  |
| SimpleDB, BatchPutAttributes         |                       |                293 ms  |


