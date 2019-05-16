//
//  FBInterface.m
//  Unity-iPhone
//
//  Created by Roy on 2019/1/15.
//

#import <Foundation/Foundation.h>
#import "FBInterface.h"
#import "FBShareMgr.h"

#import <FBSDKCoreKit/FBSDKCoreKit.h>
#import <FBSDKLoginKit/FBSDKLoginKit.h>

@implementation FBInterface

#ifdef __cplusplus
extern C"
{
#endif

    void _InitFaceBook()
    {
        
    }
    
    FBShareMgr *sharMgr = nil;
    void _DoFBShare()
    {
        if(sharMgr == nil)
        {
            sharMgr = [[FBShareMgr alloc] init];
        }
        [sharMgr onFBShare];
    }
    
    void _DoFBLogin()
    {
        NSLog(@"[iOS Native] call facebook login");
        NSString *str = @"";
        FBSDKLoginManager *login = [[FBSDKLoginManager alloc] init];
        [login
         logOut
         ];
        [login
         logInWithReadPermissions: @[@"public_profile",@"email"]
         handler:^(FBSDKLoginManagerLoginResult *result, NSError *error) {
             if (error) {
                 NSLog(@"Process error");
                // [login logOut];
                 UnitySendMessage("UIRoot", "OnFBCancelLogin", str);
             } else if (result.isCancelled) {
                 NSLog(@"Cancelled");
                 UnitySendMessage("UIRoot", "OnFBCancelLogin", str);
             } else {
                 NSLog(@"Logged in");
                 NSDictionary *num = [[NSDictionary alloc] initWithObjectsAndKeys:FBSDKAccessToken.currentAccessToken.tokenString,@"token", FBSDKAccessToken.currentAccessToken.userID, @"uid", nil];
                 UnitySendMessage("UIRoot", "OnFBLoginBack", [[FBInterface dictionaryToJson:num] UTF8String]);
             }
         }];
    }
    
    void _DoFBLogout()
    {
        
    }
    
    void _DoFBEvent(void *p)
    {
        NSLog(@"unity call ios fb logevt start, %@", [NSString stringWithUTF8String:p]);
        NSString *list = [NSString stringWithUTF8String:p];
        NSArray *idArray = [list componentsSeparatedByString:@","];    NSSet *idSet = [NSSet setWithArray:idArray];
        NSString *price = [idArray objectAtIndex:0];
        NSString *currency = [idArray objectAtIndex:2];
        NSString *bundleid = [idArray objectAtIndex:1];
        NSDictionary *params =
        @{
          @"price" : price,
          @"currency" : currency,
          @"bundleid" : bundleid,
          };
        [FBSDKAppEvents
         logEvent:@"iosfbPurchase"
         parameters:params];
        NSLog(@"ios fb logevt success");
    }
#ifdef __cplusplus
} //extern "C"
#endif

+ (NSString*)dictionaryToJson:(NSDictionary *)dic
{
    
    NSError *parseError = nil;
    
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dic options:NSJSONWritingPrettyPrinted error:&parseError];
    
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}
@end
