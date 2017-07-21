package com.highwayns.ht.ui.adapter;

import android.content.Context;
import android.graphics.drawable.Drawable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;
import com.highwayns.ht.R;
import com.highwayns.ht.utils.ListItem;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by highwayns on 2017/7/21.
 */
public class ListViewAdapter extends BaseAdapter {
    public List<ListItem> mList= new ArrayList<>();
    private Context mContext = null;


    public ListViewAdapter(Context context){
        mContext = context;
    }

    @Override
    public int getCount() {
        return mList.size();
    }

    @Override
    public Object getItem(int i) {
        return mList.get(i);
    }

    @Override
    public long getItemId(int i) {
        return i;
    }

    @Override
    public View getView(int i, View view, ViewGroup viewGroup) {
        ListItemView listItemView;
        //初始化View
        if(view == null){
            //通过LayoutInflater实例化xml中定义的View
            view = LayoutInflater.from(mContext).inflate(R.layout.list_view_items, null);

            //实例化封装类ListItemView
            listItemView = new ListItemView();
            listItemView.imageView = (ImageView)view.findViewById(R.id.list_view_items_ivIcon);
            listItemView.textView = (TextView)view.findViewById(R.id.list_view_items_tvTitle);

            view.setTag(listItemView);
        }else {
            listItemView = (ListItemView)view.getTag();
        }

        Drawable icon = mList.get(i).getImage();
        String title = mList.get(i).getTitle();

        listItemView.imageView.setImageDrawable(icon);
        listItemView.textView.setText(title);

        return view;
    }



    //视图组件类
    class ListItemView{
        ImageView imageView;
        TextView textView;
    }
}
