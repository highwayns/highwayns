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

 public class HomeConsultantActivity extends Activity {

     private ListView mListView;
     private ListViewAdapter listViewAdapter;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home_counselor);

        initView();
    }

     private void initView() {

         mListView = (ListView)findViewById(R.id.lvHome_Person);
         listViewAdapter = new ListViewAdapter(this);

         Resources res = this.getResources();

         //基本情报
         ListItem item = new ListItem();
         item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
         item.setTitle(res.getString(R.string.CONSULTANT_HOME_LIST_BASIC_INFO));
         listViewAdapter.mList.add(item);

         //教育履历
         item = new ListItem();
         item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
         item.setTitle(res.getString(R.string.CONSULTANT_HOME_LIST_EDU_RECORD));
         listViewAdapter.mList.add(item);

         //工作履历
         item = new ListItem();
         item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
         item.setTitle(res.getString(R.string.CONSULTANT_HOME_LIST_WORK_RECORD));
         listViewAdapter.mList.add(item);

         //企业一览
         item = new ListItem();
         item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
         item.setTitle(res.getString(R.string.CONSULTANT_HOME_LIST_ENTERPRISE_LIST));
         listViewAdapter.mList.add(item);

         //知识竞技
         item = new ListItem();
         item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
         item.setTitle(res.getString(R.string.CONSULTANT_HOME_LIST_SKILL_CONTEST));
         listViewAdapter.mList.add(item);

         //SERVICE
         item = new ListItem();
         item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
         item.setTitle(res.getString(R.string.CONSULTANT_HOME_LIST_SERVICE));
         listViewAdapter.mList.add(item);

         //个人详谈
         item = new ListItem();
         item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
         item.setTitle(res.getString(R.string.CONSULTANT_HOME_LIST_PERSONAL_CONTACT));
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
                         intent = new Intent(HomeConsultantActivity.this, ConsultantBasicInfoActivity.class);
                         startActivity(intent);
                         break;
                     //教育履历
                     case 1:
                         intent = new Intent(HomeConsultantActivity.this, ConsultantEduRecordActivity.class);
                         startActivity(intent);
                         break;
                     //工作履历
                     case 2:
                         intent = new Intent(HomeConsultantActivity.this, ConsultantWorkRecordActivity.class);
                         startActivity(intent);
                         break;
                     //企业检索
                     case 3:
                         intent = new Intent(HomeConsultantActivity.this, ConsultantEnterpriseSearchActivity.class);
                         startActivity(intent);
                         break;
                     //知识竞技
                     case 4:
                         intent = new Intent(HomeConsultantActivity.this, HomeSkillActivity.class);
                         startActivity(intent);
                         break;
                     //SERVICE
                     case 5:
                         intent = new Intent(HomeConsultantActivity.this, HomeServiceActivity.class);
                         startActivity(intent);
                         break;
                     //顾问详谈
                     case 6:
                         //intent = new Intent(HomeConsultantActivity.this, ConsultantPersonalContactActivity.class);
                         //startActivity(intent);
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
