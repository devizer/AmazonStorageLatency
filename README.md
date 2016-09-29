## Amazon Storage Latency
Q: What is latency of SimpleDB, DynamoDB, Simple Storage, Simplae Queue and AIM?

|      Operation                       | US-WEST-2, Same Region<p>Moderte network   | EE                     |
| :-------                             |                                      ---:  |                   ---: |
| DynamoDB, BatchWriteItem             |                                      43 ms |                229 ms  |
| DynamoDB, ListTables                 |                                      20 ms |                233 ms  |
| SimpleDB, BatchPutAttributes         |                                      42 ms |                293 ms  |


