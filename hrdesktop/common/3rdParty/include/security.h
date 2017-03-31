/*================================================================
*   Copyright (C) 2015 All rights reserved.
*   
*   鏂囦欢鍚嶇О锛歴ecurity.h
*   鍒?寤?鑰咃細Zhang Yuanhao
*   閭?   绠憋細bluefoxah@gmail.com
*   鍒涘缓鏃ユ湡锛?015骞?1鏈?9鏃?
*   鎻?   杩帮細
*
#pragma once
================================================================*/

#ifndef __SECURITY_H__
#define __SECURITY_H__


#ifdef __cplusplus
extern "C" {
#endif
    
#ifdef __ANDROID__
    jstring Java_com_mogujie_im_security_EncryptMsg(JNIEnv* env, jobject obj, jstring jstr);
    jstring Java_com_mogujie_im_security_DecryptMsg(JNIEnv* env, jobject obj, jstring jstr);
    jstring Java_com_mogujie_im_security_EncryptPass(JNIEnv* env, jobject obj, jstring jstr, jstring jkey);

#else
    /**
     *  瀵规秷鎭姞瀵?
     *
     *  @param pInData  寰呭姞瀵嗙殑娑堟伅鍐呭鎸囬拡
     *  @param nInLen   寰呭姞瀵嗘秷鎭唴瀹归暱搴?
     *  @param pOutData 鍔犲瘑鍚庣殑鏂囨湰
     *  @param nOutLen  鍔犲瘑鍚庣殑鏂囨湰闀垮害
     *
     *  @return 杩斿洖 0-鎴愬姛; 鍏朵粬-澶辫触
     */
    int EncryptMsg(const char* pInData, uint32_t nInLen, char** pOutData, uint32_t& nOutLen);
    
    /**
     *  瀵规秷鎭В瀵?
     *
     *  @param pInData  寰呰В瀵嗙殑娑堟伅鍐呭鎸囬拡
     *  @param nInLen   寰呰В瀵嗘秷鎭唴瀹归暱搴?
     *  @param pOutData 瑙ｅ瘑鍚庣殑鏂囨湰
     *  @param nOutLen  瑙ｅ瘑鍚庣殑鏂囨湰闀垮害
     *
     *  @return 杩斿洖 0-鎴愬姛; 鍏朵粬-澶辫触
     */
    int DecryptMsg(const char* pInData, uint32_t nInLen, char** pOutData, uint32_t& nOutLen);
    
    /**
     *  瀵瑰瘑鐮佽繘琛屽姞瀵?
     *
     *  @param pInData  寰呰В瀵嗙殑娑堟伅鍐呭鎸囬拡
     *  @param nInLen   寰呰В瀵嗘秷鎭唴瀹归暱搴?
     *  @param pOutData 瑙ｅ瘑鍚庣殑鏂囨湰
     *  @param nOutLen  瑙ｅ瘑鍚庣殑鏂囨湰闀垮害
     *  @param pKey     32浣嶅瘑閽?
     *
     *  @return 杩斿洖 0-鎴愬姛; 鍏朵粬-澶辫触
     */
    int EncryptPass(const char* pInData, uint32_t nInLen, char** pOutData, uint32_t& nOutLen);
    
    /**
     *  閲婃斁璧勬簮
     *
     *  @param pOutData 闇€瑕侀噴鏀剧殑璧勬簮
     */
    void Free(char* pOutData);
    
#endif
    
#ifdef __cplusplus
}
#endif

#endif
