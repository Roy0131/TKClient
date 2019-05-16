//
//  DriverInterface.m
//  Unity-iPhone
//
//  Created by roy on 16/10/21.
//
//
#import "DriverInterface.h"
#import <Foundation/Foundation.h>

#ifdef __cplusplus
extern "C"
{
#endif
    
    //复制到剪贴板
    void _PasteBoard(char *str)
    {
        NSString *strReadAddr = [NSString stringWithUTF8String:str];
        UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
        pasteboard.string = strReadAddr;
    }
    
//    // ios手机的当前语言 "en"、“zh"、“zh-Hans"、"zh-Hant"
//    const char* _CurIOSLang()
//    {
//        NSArray *languages = [NSLocale preferredLanguages];
//        NSString *currentLanguage = [languages objectAtIndex:0];
//        return cStringCopy([currentLanguage UTF8String]);
//    }
#ifdef __cplusplus
} // extern "C"
#endif

