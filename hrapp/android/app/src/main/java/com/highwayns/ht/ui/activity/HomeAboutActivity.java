package com.highwayns.ht.ui.activity;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Resources;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ListView;
import com.highwayns.ht.R;
import com.highwayns.ht.ui.adapter.ListViewAdapter;
import com.highwayns.ht.utils.ListItem;

import static android.content.ContentValues.TAG;

public class HomeAboutActivity extends Activity {

    private ListViewAdapter listViewAdapter;
    private ListView mListView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home_company);

        initView();
    }

    private void initView() {

        mListView = (ListView)findViewById(R.id.lvHome_Person);
        listViewAdapter = new ListViewAdapter(this);

        Resources res = this.getResources();

        //基本情报
        ListItem item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ABOUT_HOME_LIST_BASIC_INFO));
        listViewAdapter.mList.add(item);

        //会社沿革
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ABOUT_HOME_LIST_EXP));
        listViewAdapter.mList.add(item);

        //事业内容
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ABOUT_HOME_LIST_BUSINESS));
        listViewAdapter.mList.add(item);

        //Access
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ABOUT_HOME_LIST_ACCESS));
        listViewAdapter.mList.add(item);

        //咨询
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ABOUT_HOME_LIST_CONSULTING));
        listViewAdapter.mList.add(item);

        //Partner 招募
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ABOUT_HOME_LIST_PARTNER_RECRUIT));
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
                        intent = new Intent(HomeAboutActivity.this, AboutBasicInfoActivity.class);
                        startActivity(intent);
                        break;
                    //会社沿革
                    case 1:
                        intent = new Intent(HomeAboutActivity.this, AboutSocietyEvoActivity.class);
                        startActivity(intent);
                        break;
                    //事业内容
                    case 2:
                        intent = new Intent(HomeAboutActivity.this, AboutJobInfoActivity.class);
                        startActivity(intent);
                        break;
                    //Access
                    case 3:
                        intent = new Intent(HomeAboutActivity.this, AboutAccessActivity.class);
                        startActivity(intent);
                        break;
                    //咨询
                    case 4:
                        intent = new Intent(HomeAboutActivity.this, AboutConsultingActivity.class);
                        startActivity(intent);
                        break;
                    //Partner招募
                    case 5:
                        intent = new Intent(HomeAboutActivity.this, AboutPartnerRecruitActivity.class);
                        startActivity(intent);
                        break;

                }

            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        return super.onCreateOptionsMenu(menu);
    }
}
