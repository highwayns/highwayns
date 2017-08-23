package com.highwayns.ht.ui.activity;

import android.app.Activity;
import android.content.res.Resources;
import android.os.Bundle;
import android.widget.ListView;
import com.highwayns.ht.R;
import com.highwayns.ht.ui.adapter.ThirdListAdapter;
import com.highwayns.ht.utils.ListItem;
import com.highwayns.ht.utils.ThirdListItem;

public class AboutBasicInfoActivity extends Activity {

    private ThirdListAdapter thirdListAdapter;
    private ListView listView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_company_info_aboutus);

        initView();
    }

    private void initView() {

        listView = (ListView)findViewById(R.id.lvItem_Company_Info);
        thirdListAdapter = new ThirdListAdapter(this);

        Resources res = this.getResources();

        //会社名字
        ThirdListItem item = new ThirdListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ABOUT_BASIC_INFO_LIST_COMPANY_NAME));
        thirdListAdapter.mList.add(item);

        //住所
        item = new ThirdListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ABOUT_BASIC_INFO_LIST_ADDR));
        thirdListAdapter.mList.add(item);

        //取缔役
        item = new ThirdListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ABOUT_BASIC_INFO_LIST_BOARD_DIRECTOR));
        thirdListAdapter.mList.add(item);

        //资本金
        item = new ThirdListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ABOUT_BASIC_INFO_LIST_REG_FUND));
        thirdListAdapter.mList.add(item);

        //经营范围
        item = new ThirdListItem();
        item.setImage(res.getDrawable(R.drawable.tt_ic_launcher));
        item.setTitle(res.getString(R.string.ABOUT_BASIC_INFO_LIST_BUSINESS_CONTENT));
        thirdListAdapter.mList.add(item);

        listView.setAdapter(thirdListAdapter);
    }
}
