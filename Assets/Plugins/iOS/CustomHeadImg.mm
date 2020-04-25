@interface MY_HeadImage : UIViewController<UIImagePickerControllerDelegate,UINavigationControllerDelegate>

+(MY_HeadImage *)sharedInstance;
-(void)MenuSelect;

@end
//暴露接口，供C#调用
extern "C" void MY_OpenHeadImage(){[[MY_HeadImage sharedInstance] MenuSelect];}

@implementation MY_HeadImage
static MY_HeadImage *instance = nil;

UIViewController * selfView;

+(MY_HeadImage *)sharedInstance{
    @synchronized(self) {
        if(instance == nil) {
            instance = [[[self class] alloc] init];
            selfView = UnityGetGLViewController();
        }
    }
    return instance;
}

-(void)MenuSelect{
    UIAlertController * alertController = [UIAlertController alertControllerWithTitle:@"选择头像" message:@"" preferredStyle:UIAlertControllerStyleActionSheet];
   // UIAlertControllerStyleAlert在中央屏幕。
   // UIAlertControllerStyleActionSheet在屏幕底部。
    UIAlertAction *useCamera = [UIAlertAction actionWithTitle:@"相机" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        NSLog(@"拍照");
        [instance pickFromCamera];
    }];
    UIAlertAction *usePhoto = [UIAlertAction actionWithTitle:@"相册" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        NSLog(@"相册");
        [instance pickFromAlbum];
    }];
    UIAlertAction *cancelAction = [UIAlertAction actionWithTitle:@"取消" style:UIAlertActionStyleCancel handler:nil];
    [alertController addAction:useCamera];
    [alertController addAction:usePhoto];
    [alertController addAction:cancelAction];
    [selfView presentViewController:alertController animated:YES completion:nil];
}

//从相机选择
-(void)pickFromCamera
{
    UIImagePickerController *imagePicker = [[UIImagePickerController alloc] init];
    imagePicker.delegate = self;
    imagePicker.allowsEditing = YES;
    imagePicker.sourceType = UIImagePickerControllerSourceTypeCamera;
    [selfView presentViewController:imagePicker animated:YES completion:nil];
}

//从相册选择
-(void)pickFromAlbum
{
    UIImagePickerController *imagePicker = [[UIImagePickerController alloc] init];
    imagePicker.delegate = self;
    imagePicker.allowsEditing = YES;
    imagePicker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    [selfView presentViewController:imagePicker animated:YES completion:nil];
}

//选择完成回调（系统自己调用的,别找了）
- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info
{
    NSLog(@"选择头像完成");
    UIImage *img = [info objectForKey:UIImagePickerControllerEditedImage];
    [self performSelector:@selector(saveImage:)  withObject:img afterDelay:0.5];
    [picker dismissViewControllerAnimated:YES completion:nil];
}

//保存图片
- (void)saveImage:(UIImage *)image {
    BOOL success;
    NSFileManager *fileManager = [NSFileManager defaultManager];
    NSError *error;

    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *documentsDirectory = [paths objectAtIndex:0];
    NSString *imageFilePath = [documentsDirectory stringByAppendingPathComponent:@"image.jpg"];
    NSLog(@"头像写入[IOS]->>%@",imageFilePath);
    success = [fileManager fileExistsAtPath:imageFilePath];
    if(success) {
        success = [fileManager removeItemAtPath:imageFilePath error:&error];
    }
    UIImage *smallImage = [self thumbnailWithImageWithoutScale:image size:CGSizeMake(93, 93)];
    [UIImageJPEGRepresentation(smallImage, 1.0f) writeToFile:imageFilePath atomically:YES];//写入文件
    UnitySendMessage("Camera", "PickHeadImgSucc", "image.jpg");
}

//保持原来的长宽比，生成一个缩略图
- (UIImage *)thumbnailWithImageWithoutScale:(UIImage *)image size:(CGSize)asize
{
    UIImage *newimage;
    if (nil == image) {
        newimage = nil;
    }
    else{
        CGSize oldsize = image.size;
        CGRect rect;
        if (asize.width/asize.height > oldsize.width/oldsize.height) {
            rect.size.width = asize.height*oldsize.width/oldsize.height;
            rect.size.height = asize.height;
            rect.origin.x = (asize.width - rect.size.width)/2;
            rect.origin.y = 0;
        }
        else{
            rect.size.width = asize.width;
            rect.size.height = asize.width*oldsize.height/oldsize.width;
            rect.origin.x = 0;
            rect.origin.y = (asize.height - rect.size.height)/2;
        }
        UIGraphicsBeginImageContext(asize);
        CGContextRef context = UIGraphicsGetCurrentContext();
        CGContextSetFillColorWithColor(context, [[UIColor clearColor] CGColor]);
        UIRectFill(CGRectMake(0, 0, asize.width, asize.height));//clear background
        [image drawInRect:rect];
        newimage = UIGraphicsGetImageFromCurrentImageContext();
        UIGraphicsEndImageContext();
    }
    return newimage;
}


@end
