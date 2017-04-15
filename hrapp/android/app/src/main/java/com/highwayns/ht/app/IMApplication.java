package com.highwayns.ht.app;

import android.app.Application;
import android.content.Intent;
import com.highwayns.ht.imservice.service.IMService;
import com.highwayns.ht.utils.ImageLoaderUtil;
import com.highwayns.ht.utils.Logger;


public class IMApplication extends Application {

	private Logger logger = Logger.getLogger(IMApplication.class);

	/**
 	 * test
	 * @param args
	 */
	public static void main(String[] args) {
	}

	@Override
	public void onCreate() {
		super.onCreate();
		logger.i("Application starts");
		startIMService();
		ImageLoaderUtil.initImageLoaderConfig(getApplicationContext());
	}

	private void startIMService() {
		logger.i("start IMService");
		Intent intent = new Intent();
		intent.setClass(this, IMService.class);
		startService(intent);
	}

    public static boolean gifRunning = true;//gif是否运行
}
