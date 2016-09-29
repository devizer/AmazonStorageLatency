## Amazon Storage Latency
Q: What is latency of SimpleDB, DynamoDB, Simple Storage, Simplae Queue and AIM?

|      Operation                       | US-WEST-2, 10 GBit | US-WEST-2, Moderte network   | EE                     |
| :-------                             |              ---:  |                 ---:  |                   ---: |
| ping s3-us-west-2.amazonaws.com      |              <1 ms |                 <1 ms |                190 ms  |
| DynamoDB, BatchWriteItem             |               7 ms |                 43 ms |                229 ms  |
| DynamoDB, ListTables                 |               5 ms |                 20 ms |                233 ms  |
| SimpleDB, BatchPutAttributes         |              28 ms |                 42 ms |                293 ms  |
| S3, PutObject (new)                  |              13 ms |                 15 ms |                519 ms  |
| S3, PutObject (update)               |              10 ms |                 25 ms |                499 ms  |
| S3, GetObjects                       |              11 ms |                  7 ms |                307 ms  |
| S3, ListObjects                      |              11 ms |                 11 ms |                266 ms  |
| S3, DeleteObjects                    |              12 ms |                 12 ms |                244 ms  |
