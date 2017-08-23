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

public class HomeServiceActivity extends Activity {

    private ListView mListView;
    private ListViewAdapter listViewAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home_service);

        initView();
    }

    private void initView() {
        mListView = (ListView)findViewById(R.id.lvItem_Service_Home);
        listViewAdapter = new ListViewAdapter(this);

        Resources res = this.getResources();

        //房屋租赁
        ListItem item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.SERVICE_HOME_LIST_RENT));
        listViewAdapter.mList.add(item);

        //房屋修理
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.SERVICE_HOME_LIST_REPAIR));
        listViewAdapter.mList.add(item);

        //机票预定
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.SERVICE_HOME_LIST_BOOK_AIR_TICKET));
        listViewAdapter.mList.add(item);

        //酒店预定
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.SERVICE_HOME_LIST_HOTEL));
        listViewAdapter.mList.add(item);

        //食事预约
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.SERVICE_HOME_LIST_RESTAURANT_RESERVATION));
        listViewAdapter.mList.add(item);

        //Event
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.SERVICE_HOME_LIST_EVENT));
        listViewAdapter.mList.add(item);

        mListView.setAdapter(listViewAdapter);

        mListView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long l) {
                Log.d(TAG,"onItemClickListener position="+ position);
                Intent intent;
                switch (position){
                    //房屋租赁
                    case 0:
                        //intent = new Intent(HomeServiceActivity.this, PersonalBasicInfoActivity.class);
                        //startActivity(intent);
                        break;
                    //房屋修理
                    case 1:
                        //intent = new Intent(HomeServiceActivity.this, PersonalEduRecordActivity.class);
                        //startActivity(intent);
                        break;
                    //机票预定
                    case 2:
                        //intent = new Intent(HomeServiceActivity.this, PersonalWorkRecordActivity.class);
                        //startActivity(intent);
                        break;
                    //酒店预定
                    case 3:
                        //intent = new Intent(HomeServiceActivity.this, PersonalJobSearchActivity.class);
                        //startActivity(intent);
                        break;
                    //食事预约
                    case 4:
                        //intent = new Intent(HomeServiceActivity.this, HomeSkillActivity.class);
                        //startActivity(intent);
                        break;
                    //Event
                    case 5:
                        //intent = new Intent(HomeServiceActivity.this, HomeServiceActivity.class);
                        //startActivity(intent);
                        break;
                }

            }
        });
    }
}
