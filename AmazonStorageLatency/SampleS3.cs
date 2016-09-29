namespace AmazonS3Sample
{
    using System;
    using System.IO;

    using Amazon;
    using Amazon.S3;
    using Amazon.S3.Model;

    class SampleS3
    {
        // Change the AWSProfileName to the profile you want to use in the App.config file.
        // See http://docs.aws.amazon.com/AWSSdkDocsNET/latest/DeveloperGuide/net-dg-config-creds.html for more details.
        // You must also sign up for an Amazon S3 account for this to work
        // See http://aws.amazon.com/s3/ for details on creating an Amazon S3 account
        // Change the bucketName and keyName fields to values that match your bucketname and keyname
        static string bucketName = "test-backet-by-devizer";
        static string keyName = "test-key";
        static IAmazonS3 client;

        public static void Run()
        {

            client = new AmazonS3Client(RegionEndpoint.USWest2);
            "S3\\ListBuckets".ExecAws(() => client.ListBuckets());
            "S3\\PutBucket".ExecAws(() => client.PutBucket(new PutBucketRequest() { BucketName = bucketName }));

            PutObject_Simple();
            PutObject_TitledInHeader();

            GetAnObject();

            ListingObjects();

            DeletingAnObject();
        }

        private static void PutObject_Simple()
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                ContentBody = "this is a test",
                BucketName = bucketName,
                Key = keyName,
            };

            "S3\\PutObject".ExecAws(() => client.PutObject(request));
        }

        private static void PutObject_TitledInHeader()
        {
            PutObjectRequest titledRequest = new PutObjectRequest()
            {
                ContentBody = "this is ANOTHER blob",
                BucketName = bucketName,
                Key = keyName
            };
            titledRequest.Metadata.Add("title", "the title");

            "S3\\PutObject(Titled)".ExecAws(() => client.PutObject(titledRequest));
        }



        static bool checkRequiredFields()
        {

            if (string.IsNullOrEmpty(bucketName))
            {
                Console.WriteLine("The variable bucketName is not set.");
                return false;
            }
            if (string.IsNullOrEmpty(keyName))
            {
                Console.WriteLine("The variable keyName is not set.");
                return false;
            }

            return true;
        }


        static void CreateABucket()
        {
            try
            {
                PutBucketRequest request = new PutBucketRequest();
                request.BucketName = bucketName;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("An Error, number {0}, occurred when creating a bucket with the message '{1}", amazonS3Exception.ErrorCode, amazonS3Exception.Message);
                }
            }
        }

        static void WritingAnObject()
        {
            try
            {
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
                }
            }
        }


        private static void GetAnObject()
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = keyName
            };


            var response = "S3\\GetObject".ExecAws(() => client.GetObject(request));
            if (response != null)
            {
                string title = response.Metadata["x-amz-meta-title"];
                Console.WriteLine("The object's title is {0}", title);
                string dest = Path.Combine("Logs\\S3", keyName);
                if (!File.Exists(dest))
                {
                    response.WriteResponseStreamToFile(dest);
                }
            }
        }

        static void DeletingAnObject()
        {
            DeleteObjectRequest request = new DeleteObjectRequest()
            {
                BucketName = bucketName,
                Key = keyName
            };

            "S3\\DeleteObject".ExecAws(() => client.DeleteObject(request));
/*
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("An error occurred with the message '{0}' when deleting an object", amazonS3Exception.Message);
                }
            }
*/
        }

        static void ListingObjects()
        {
            try
            {
                ListObjectsRequest request = new ListObjectsRequest();
                request.BucketName = bucketName;
                ListObjectsResponse response =  "S3\\ListObjects[All]".ExecAws(() => client.ListObjects(request));
                foreach (S3Object entry in response.S3Objects)
                {
                    Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
                }

                // list only things starting with "foo"
                request.Prefix = "foo";
                response = "S3\\ListObjects[starting with foo...]".ExecAws(() => client.ListObjects(request));
                foreach (S3Object entry in response.S3Objects)
                {
                    Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
                }

                // list only things that come after "bar" alphabetically
                request.Prefix = null;
                request.Marker = "bar";
                response = "S3\\ListObjects[after bar alphabetically]".ExecAws( () => client.ListObjects(request));
                foreach (S3Object entry in response.S3Objects)
                {
                    Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
                }

                // only list 3 things
                request.Prefix = null;
                request.Marker = null;
                request.MaxKeys = 3;
                response = "S3\\ListObjects[only 3 files]".ExecAws(() => client.ListObjects(request));
                foreach (S3Object entry in response.S3Objects)
                {
                    Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("An error occurred with the message '{0}' when listing objects", amazonS3Exception.Message);
                }
            }
        }
    }
}
