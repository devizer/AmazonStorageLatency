using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonStorageLatency
{
    using Amazon;
    using Amazon.IdentityManagement;
    using Amazon.IdentityManagement.Model;

    class SampleIdentity
    {
        public static void Run()
        {
            var iam = new AmazonIdentityManagementServiceClient(RegionEndpoint.USWest2);

            "IAM\\ListAccessKeys()".ExecAws(() => iam.ListAccessKeys(new ListAccessKeysRequest()));

            // ListUsers
            var usersResponse = "IAM\\ListUsers".ExecAws(() => iam.ListUsers());
            var users = usersResponse.Users;
            Console.WriteLine("Total Users: " + users.Count);

            foreach (var userInfo in users)
            {
                var userName = userInfo.UserName;
                Console.WriteLine("  USER {0}:", userName);

                // GetUser
                var getUserResponse = "IAM\\GetUser".ExecAws(() => 
                    iam.GetUser(new GetUserRequest() {UserName = userName})
                    );
                var userDetail = getUserResponse.User;

                // ListGroupsForUser, truncated
                var rGroupsForUser = ("IAM\\GetGroupsForUser-" + userName).ExecAws(() =>
                    iam.ListGroupsForUser(new ListGroupsForUserRequest(userName))
                    );
                var groupsForUser = rGroupsForUser.Groups;
                foreach (var group in groupsForUser)
                {
                    Console.WriteLine("    group {0}", group.GroupName);
                }

                var attachedPolicies = "IAM\\ListAttachedUserPolicies".ExecAws(() => iam.ListAttachedUserPolicies(new ListAttachedUserPoliciesRequest() { UserName = userName}));
                if (attachedPolicies != null)
                {
                    foreach (var apRef in attachedPolicies.AttachedPolicies)
                    {
                        Console.WriteLine("    attached policy {0}, '{1}'", apRef.PolicyArn, apRef.PolicyName);
                        var p = "IAM\\GetPolicy".ExecAws(() => iam.GetPolicy(new GetPolicyRequest() { PolicyArn = apRef.PolicyArn }));
                    }
                }

                var pl = "IAM\\ListUserPolicies".ExecAws(() => iam.ListUserPolicies(new ListUserPoliciesRequest(userName)));
                if (pl != null)
                {
                    foreach (var apRef in pl.PolicyNames)
                    {
                        Console.WriteLine("    policy {0}, '{1}'", apRef);
                        // var p = "IAM\\GetPolicy".ExecAws(() => iam.GetPolicy(new GetPolicyRequest() { PolicyArn = apRef.PolicyArn }));
                    }
                }

                var actionName = "IAM\\ListAccessKeys(" + userName + ")";
                actionName.ExecAws(() => iam.ListAccessKeys(new ListAccessKeysRequest() { UserName = userName }));
/*
                foreach (var policy in user.GetPolicies())
                {
                    Console.WriteLine("    {0}", policy.Name);
                }

                Console.WriteLine("  Access keys:");

                foreach (var accessKey in user.GetAccessKeys())
                {
                    Console.WriteLine("    {0}", accessKey.Id);
                }
*/
            }
        }
    }
}
