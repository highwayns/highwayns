package com.highwayns.ht.ui.activity;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Resources;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ListView;
import com.highwayns.ht.R;
import com.highwayns.ht.ui.adapter.ListViewAdapter;
import com.highwayns.ht.utils.ListItem;

import static android.content.ContentValues.TAG;

public class HomeUserCenterActivity extends Activity {

    private ListView mListView;
    private ListViewAdapter listViewAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home_usercenter);

        initView();
    }

    private void initView() {
        mListView = (ListView) findViewById(R.id.lvUserCenter_Person);
        listViewAdapter = new ListViewAdapter(this);

        Resources res = this.getResources();

        //Login/Logout
        ListItem item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.USER_CENTER_HOME_LIST_LOGIN));
        listViewAdapter.mList.add(item);

        //基本情报
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.USER_CENTER_HOME_LIST_BASIC_INFO));
        listViewAdapter.mList.add(item);

        //Change Password
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.USER_CENTER_HOME_LIST_CHANGE_PWD));
        listViewAdapter.mList.add(item);

        //Point Management
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.USER_CENTER_HOME_LIST_POINT_MANAGE));
        listViewAdapter.mList.add(item);

        //Friends Recommend
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.USER_CENTER_HOME_LIST_FRIEND_RECOMMEND));
        listViewAdapter.mList.add(item);

        //Forgot Password
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.USER_CENTER_HOME_LIST_FORGOT_PWD));
        listViewAdapter.mList.add(item);

        //System settings
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.USER_CENTER_HOME_LIST_SYS_SETTING));
        listViewAdapter.mList.add(item);

        mListView.setAdapter(listViewAdapter);

        mListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long l) {
                Log.d(TAG, "onItemClickListener position=" + position);
                Intent intent;
                switch (position) {
                    //
                    case 0:
                        intent = new Intent(HomeUserCenterActivity.this, UserCenterLoginActivity.class);
                        startActivity(intent);
                        break;
                    //
                    case 1:
                        intent = new Intent(HomeUserCenterActivity.this, UserCenterBasicInfoActivity.class);
                        startActivity(intent);
                        break;
                    //
                    case 2:
                        intent = new Intent(HomeUserCenterActivity.this, UserCenterChgPwdActivity.class);
                        startActivity(intent);
                        break;
                    //
                    case 3:
                        intent = new Intent(HomeUserCenterActivity.this, UserCenterPointActivity.class);
                        startActivity(intent);
                        break;
                    //
                    case 4:
                        intent = new Intent(HomeUserCenterActivity.this, UserCenterRecmdActivity.class);
                        startActivity(intent);
                        break;
                    //
                    case 5:
                        intent = new Intent(HomeUserCenterActivity.this, UserCenterForgotPwdActivity.class);
                        startActivity(intent);
                        break;
                    //
                    case 6:
                        intent = new Intent(HomeUserCenterActivity.this, UserCenterSettingsActivity.class);
                        startActivity(intent);
                        break;
                }

            }
        });
    }
}

