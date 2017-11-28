//
//  MCLPreviewConfig.h
//
//  An online version of the documentation can be found at https://docs.megacool.co
//
//  Do you feel like this reading docs? (╯°□°）╯︵ ┻━┻  Give us a shout if there's anything that's
//  unclear and we'll try to brighten the experience. We can sadly not replace flipped tables.
//

#ifndef MCLPreviewConfig_h
#define MCLPreviewConfig_h

/*!
 @brief Configures a preview.
 */
@interface MCLPreviewConfig : NSObject

/**
 @brief Which recording to create a preview for.
 */
@property NSString *recordingId;

/**
 @brief Where in the view the preview should be.
 */
@property CGRect previewFrame;

/**
 @brief Whether to include the last frame overlay in the preview or not. Default is false.
 */
@property BOOL includeLastFrameOverlay;

@end

#endif /* MCLPreviewConfig_h */
