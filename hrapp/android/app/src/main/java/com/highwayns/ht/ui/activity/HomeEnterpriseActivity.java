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

/**
 * 企业homepage
 * @author
 */
public class HomeEnterpriseActivity extends Activity {

    private ListView mListView;
    private ListViewAdapter listViewAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_enterprise_home);

        initView();
    }

    private void initView() {

        mListView = (ListView)findViewById(R.id.lvEnterprise_Person);
        listViewAdapter = new ListViewAdapter(this);

        Resources res = this.getResources();

        //基本情报
        ListItem item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ENTERPRISE_HOME_LIST_BASIC_INFO));
        listViewAdapter.mList.add(item);

        //会社沿革
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ENTERPRISE_HOME_LIST_EXP));
        listViewAdapter.mList.add(item);

        //事业内容
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ENTERPRISE_HOME_LIST_BUSINESS));
        listViewAdapter.mList.add(item);

        //仕事配布
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ENTERPRISE_HOME_LIST_JOB_INDICATE));
        listViewAdapter.mList.add(item);

        //顾问一览
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ENTERPRISE_HOME_LIST_CONSULTANT));
        listViewAdapter.mList.add(item);

        //SERVICE
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ENTERPRISE_HOME_LIST_SERVICE));
        listViewAdapter.mList.add(item);

        //个人一览
        item = new ListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ENTERPRISE_HOME_LIST_PERSONAL_CONTACT));
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
                        intent = new Intent(HomeEnterpriseActivity.this, EnterpriseBasicInfoActivity.class);
                        startActivity(intent);
                        break;
                    //会社沿革
                    case 1:
                        intent = new Intent(HomeEnterpriseActivity.this, EnterpriseExpActivity.class);
                        startActivity(intent);
                        break;
                    //事业内容
                    case 2:
                        intent = new Intent(HomeEnterpriseActivity.this, EnterpriseBusinessActivity.class);
                        startActivity(intent);
                        break;
                    //仕事配布
                    case 3:
                        intent = new Intent(HomeEnterpriseActivity.this, EnterpriseJobIndicateActivity.class);
                        startActivity(intent);
                        break;
                    //顾问一览
                    case 4:
                        intent = new Intent(HomeEnterpriseActivity.this, EnterpriseConsultantActivity.class);
                        startActivity(intent);
                        break;
                    //SERVICE
                    case 5:
                        intent = new Intent(HomeEnterpriseActivity.this, HomeServiceActivity.class);
                        startActivity(intent);
                        break;
                    //个人一览
                    case 6:
                        intent = new Intent(HomeEnterpriseActivity.this, EnterprisePersonalActivity.class);
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
