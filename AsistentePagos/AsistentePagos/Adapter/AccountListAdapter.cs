using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AsistentePagos.Core.Models;

namespace AsistentePagos.Adapter
{
    class AccountListAdapter : BaseAdapter
    {

        Context context;
        List<Account> accountList;
        TextView accountName;
        TextView accountType;
        TextView accountNumber;

        public AccountListAdapter(Context context, List<Account> listAccount)
        {
            this.context = context;
            this.accountList = listAccount;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            AccountListAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as AccountListAdapterViewHolder;

            if (holder == null)
            {
                holder = new AccountListAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();

                Account account = accountList[position];

                view = inflater.Inflate(Resource.Layout.AccountListItem, parent, false);
                InitComponents(view);
                accountName.Text = account.AccountName;
                accountType.Text = account.AccountType;
                accountNumber.Text = account.AccountNumber;

                view.Tag = holder;
            }


            //fill in your items
            //holder.Title.Text = "new text here";

            return view;
        }

        private void InitComponents(View view)
        {
            accountName = view.FindViewById<TextView>(Resource.Id.textViewAccountName);
            accountType = view.FindViewById<TextView>(Resource.Id.textViewAccountType);
            accountNumber = view.FindViewById<TextView>(Resource.Id.textViewAccountType);
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return accountList.Count;
            }
        }

    }

    class AccountListAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}