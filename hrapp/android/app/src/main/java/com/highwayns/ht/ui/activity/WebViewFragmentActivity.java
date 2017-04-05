package com.highwayns.ht.ui.activity;

import android.content.Intent;
import android.os.Bundle;

import com.highwayns.ht.R;
import com.highwayns.ht.config.IntentConstant;
import com.highwayns.ht.ui.base.TTBaseFragmentActivity;
import com.highwayns.ht.ui.fragment.WebviewFragment;

public class WebViewFragmentActivity extends TTBaseFragmentActivity {
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		Intent intent=getIntent();
		if (intent.hasExtra(IntentConstant.WEBVIEW_URL)) {
			WebviewFragment.setUrl(intent.getStringExtra(IntentConstant.WEBVIEW_URL));
		}
		setContentView(R.layout.tt_fragment_activity_webview);
	}

	@Override
	protected void onDestroy() {
		super.onDestroy();
	}
}
