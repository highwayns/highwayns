package com.highwayns.ht.ui.activity;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import com.highwayns.ht.R;

public class MainPersonalActivity extends Activity implements View.OnClickListener{

    private Button mPersonal;
    private Button mConsultant;
    private Button mCompany;
    private Button mSkillTest;
    private Button mUserCenter;
    private Button mService;
    private Button mNews;
    private Button mAbout;
    private Button mHelp;
    private ImageView mImage;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main_personal);

        initView();
    }

    //Init View
    private void initView() {
        //bind View
        mPersonal = (Button)findViewById(R.id.btnPerson_Main_Personal);
        mConsultant = (Button)findViewById(R.id.btnCounselor_Main_Personal);
        mCompany = (Button)findViewById(R.id.btnCompany_Main_Personal);
        mSkillTest = (Button)findViewById(R.id.btnSkill_Main_Personal);
        mUserCenter = (Button) findViewById(R.id.btnUserCenter_Main_Personal);
        mService = (Button)findViewById(R.id.btnService_Main_Personal);
        mNews = (Button)findViewById(R.id.btnNews_Main_Personal);
        mAbout = (Button)findViewById(R.id.btnAbout_Main_Personal);
        mHelp = (Button)findViewById(R.id.btnHelp_Main_Personal);
        mImage = (ImageView)findViewById(R.id.ivMain_Personal);

        //Set ClickListener
        mPersonal.setOnClickListener(this);
        mConsultant.setOnClickListener(this);
        mCompany.setOnClickListener(this);
        mSkillTest.setOnClickListener(this);
        mUserCenter.setOnClickListener(this);
        mService.setOnClickListener(this);
        mNews.setOnClickListener(this);
        mAbout.setOnClickListener(this);
        mHelp.setOnClickListener(this);

    }

    @Override
    public void onClick(View view) {
        Intent intent;
        switch (view.getId()){
            case R.id.btnPerson_Main_Personal:
                intent = new Intent(MainPersonalActivity.this, HomePersonalActivity.class);
                startActivity(intent);
                break;

            case R.id.btnCounselor_Main_Personal:
                intent = new Intent(MainPersonalActivity.this, HomeConsultantActivity.class);
                startActivity(intent);
                break;

            case R.id.btnCompany_Main_Personal:
                intent = new Intent(MainPersonalActivity.this, HomeEnterpriseActivity.class);
                startActivity(intent);
                break;

            case R.id.btnSkill_Main_Personal:
                intent = new Intent(MainPersonalActivity.this, HomeSkillActivity.class);
                startActivity(intent);
                break;

            case R.id.btnUserCenter_Main_Personal:
                intent = new Intent(MainPersonalActivity.this, HomeUserCenterActivity.class);
                startActivity(intent);
                break;

            case R.id.btnService_Main_Personal:
                intent = new Intent(MainPersonalActivity.this, HomeServiceActivity.class);
                startActivity(intent);
                break;

            case R.id.btnNews_Main_Personal:
                intent = new Intent(MainPersonalActivity.this, HomeNewsActivity.class);
                startActivity(intent);
                break;

            case R.id.btnAbout_Main_Personal:
                intent = new Intent(MainPersonalActivity.this, HomeAboutActivity.class);
                startActivity(intent);
                break;

            case R.id.btnHelp_Main_Personal:
                intent = new Intent(MainPersonalActivity.this, HomeHelpActivity.class);
                startActivity(intent);
                break;

            default:
                break;
        }
    }
}
