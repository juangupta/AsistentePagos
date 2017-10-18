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

namespace AsistentePagos.Activities
{
    class InvoiceListItemAdapter : BaseAdapter
    {

        Context context;
        ImageView imageviewCommerce;
        TextView descriptionTextView;
        TextView dueDateTextView;
        TextView paymentValueTextView;
        List<InvoiceModel> invoices;

        public InvoiceListItemAdapter(Context context, List<InvoiceModel> invoicesList)
        {
            this.context = context;
            this.invoices = invoicesList;
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
            InvoiceListItemAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as InvoiceListItemAdapterViewHolder;

            if (holder == null)
            {
                holder = new InvoiceListItemAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();

                InvoiceModel invoice = invoices[position];

                view = inflater.Inflate(Resource.Layout.InvoiceListItem, parent, false);
                InitComponents(view);
                imageviewCommerce.SetImageResource(Resource.Drawable.une_icon);
                descriptionTextView.Text = invoice.description;
                dueDateTextView.Text = invoice.dueDate.ToString();
                paymentValueTextView.Text = invoice.amount.ToString();

                view.Tag = holder;
            }


            //fill in your items
            //holder.Title.Text = "new text here";

            return view;
        }

        private void InitComponents(View view)
        {
            imageviewCommerce = view.FindViewById<ImageView>(Resource.Id.imageViewItem);
            descriptionTextView = view.FindViewById<TextView>(Resource.Id.textViewDescriptionItem);
            dueDateTextView = view.FindViewById<TextView>(Resource.Id.textViewDueDate);
            paymentValueTextView = view.FindViewById<TextView>(Resource.Id.textViewPaymentValue);
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return invoices.Count;
            }
        }

    }

    class InvoiceListItemAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}