//
//  FBShareMgr.h
//  Unity-iPhone
//
//  Created by Roy on 2019/4/15.
//

#import <Foundation/Foundation.h>
#import <FBSDKShareKit/FBSDKShareKit.h>

@interface FBShareMgr : NSObject<FBSDKSharingDelegate>
{
    NSString *url;
}

-(void)onFBShare;
@end
