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

public class HomeNewsActivity extends Activity {

    private ListViewAdapter listViewAdapter;
    private ListView mListView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activiy_home_news);

        initView();
    }

    private void initView() {

        mListView = (ListView)findViewById(R.id.lvItem_News_Home);
        listViewAdapter = new ListViewAdapter(this);

        Resources res = this.getResources();

        //会社新闻
        ListItem item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.NEWS_HOME_LIST_COMPANY));
        listViewAdapter.mList.add(item);

        //最新新闻
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.NEWS_HOME_LIST_NEW));
        listViewAdapter.mList.add(item);

        //国内新闻
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.NEWS_HOME_LIST_INLAND));
        listViewAdapter.mList.add(item);

        //国际新闻
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.NEWS_HOME_LIST_ABROAD));
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
                        //intent = new Intent(HomeNewsActivity.this, AboutBasicInfoActivity.class);
                        // startActivity(intent);
                        break;
                    //会社沿革
                    case 1:
                        //intent = new Intent(HomeNewsActivity.this, AboutSocietyEvoActivity.class);
                        //startActivity(intent);
                        break;
                    //事业内容
                    case 2:
                        //intent = new Intent(HomeNewsActivity.this, AboutJobInfoActivity.class);
                        //startActivity(intent);
                        break;
                    //Access
                    case 3:
                        //intent = new Intent(HomeNewsActivity.this, AboutAccessActivity.class);
                        //startActivity(intent);
                        break;
                }

            }
        });
    }
}
