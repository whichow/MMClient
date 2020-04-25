/** 
 *FileName:     M3EditorDelegate.cs 
 *Author:       HeJunJie 
 *Version:      1.0 
 *UnityVersion：5.6.2f1
 *Date:         2017-07-06 
 *Description:    
 *History: 
*/
namespace Game.Match3
{
    public delegate void MEventDelegate();
    public delegate void MEventDelegate<T>(T t);
    public delegate void MEventDelegate<T, U>(T t, U u);
    public delegate void MEventDelegate<T, U, V>(T t, U u, V v);

}