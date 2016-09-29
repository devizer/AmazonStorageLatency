## Amazon Storage Latency
Q: What is latency of SimpleDB, DynamoDB, Simple Storage, Simplae Queue and AIM?

|      Operation                       | US-WEST-2, 10 GBit | US-WEST-2, Same Region<p>Moderte network   | EE                     |
| :-------                             |              ---:  |                 ---:  |                   ---: |
| DynamoDB, BatchWriteItem             |               7 ms |                 43 ms |                229 ms  |
| DynamoDB, ListTables                 |               5 ms |                 20 ms |                233 ms  |
| SimpleDB, BatchPutAttributes         |              28 ms |                 42 ms |                293 ms  |


