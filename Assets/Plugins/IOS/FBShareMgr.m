//
//  FBShareMgr.m
//  Unity-iPhone
//
//  Created by Roy on 2019/4/15.
//
#import "FBShareMgr.h"

#import <FBSDKLoginKit/FBSDKLoginKit.h>
#import <FBSDKShareKit/FBSDKShareDialog.h>
#import <FBSDKShareKit/FBSDKShareLinkContent.h>
#import <FBSDKShareKit/FBSDKSharePhoto.h>
#import <FBSDKShareKit/FBSDKSharePhotoContent.h>

@implementation FBShareMgr

- (void)onFBShare {
    UIImage *image = [UIImage imageNamed: @"share.png"];
    
    FBSDKSharePhoto *photo = [[FBSDKSharePhoto alloc] init];
    photo.image = image;
    photo.userGenerated = YES;
    FBSDKSharePhotoContent *content = [[FBSDKSharePhotoContent alloc] init];
    content.photos = @[photo];
    
    FBSDKShareDialog *dialog = [[FBSDKShareDialog alloc] init];
    
    if ([[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"fbauth2://"]]){
        dialog.mode = FBSDKShareDialogModeNative;
    }
    else {
        dialog.mode = FBSDKShareDialogModeBrowser; //or FBSDKShareDialogModeAutomatic
    }
    dialog.shareContent = content;
    dialog.delegate = self;
    //dialog.fromViewController = self;
    [dialog show];
}

//FBSDKSharingDelegate
- (void)sharer:(id<FBSDKSharing>)sharer didCompleteWithResults:(NSDictionary *)results {
    NSLog(@"completed");
    UnitySendMessage("UIRoot", "OnFBShareBack", "0");
}

- (void)sharer:(id<FBSDKSharing>)sharer didFailWithError:(NSError *)error {
    NSLog(@"fail %@",error.description);
    UnitySendMessage("UIRoot", "OnFBShareBack", "-1");
}

- (void)sharerDidCancel:(id<FBSDKSharing>)sharer {
    NSLog(@"cancel");
    UnitySendMessage("UIRoot", "OnFBShareBack", "1");
}

@end
