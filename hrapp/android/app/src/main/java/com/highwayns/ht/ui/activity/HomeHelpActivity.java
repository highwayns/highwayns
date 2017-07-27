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

public class HomeHelpActivity extends Activity {

    private ListViewAdapter listViewAdapter;
    private ListView mListView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home_help);

        initView();
    }

    private void initView() {
        mListView = (ListView)findViewById(R.id.lvItem_Help);
        listViewAdapter = new ListViewAdapter(this);

        Resources res = this.getResources();

        //个人信息更新
        ListItem item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.HELP_HOME_LIST_UPDATE_INFO));
        listViewAdapter.mList.add(item);

        //顾问操作
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.HELP_HOME_LIST_CONSULTANT_OPERATION));
        listViewAdapter.mList.add(item);

        //企业人才招募
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.HELP_HOME_LIST_ENTERPRISE_RECRUITMENT));
        listViewAdapter.mList.add(item);

        //服务
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.HELP_HOME_LIST_SERVICE_APPLICATION));
        listViewAdapter.mList.add(item);

        mListView.setAdapter(listViewAdapter);

        mListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long l) {
                Log.d(TAG,"onItemClickListener position="+ position);
                Intent intent;
                switch (position){
                    //基本情报
                    case 0:
                        //intent = new Intent(HomeHelpActivity.this, AboutBasicInfoActivity.class);
                       // startActivity(intent);
                        break;
                    //会社沿革
                    case 1:
                        //intent = new Intent(HomeHelpActivity.this, AboutSocietyEvoActivity.class);
                        //startActivity(intent);
                        break;
                    //事业内容
                    case 2:
                        //intent = new Intent(HomeHelpActivity.this, AboutJobInfoActivity.class);
                        //startActivity(intent);
                        break;
                    //Access
                    case 3:
                        //intent = new Intent(HomeHelpActivity.this, AboutAccessActivity.class);
                        //startActivity(intent);
                        break;
                }

            }
        });
    }
}
