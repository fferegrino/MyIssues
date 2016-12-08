
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Object = Java.Lang.Object;

namespace MyIssues.Droid.Adapters
{
	public class ReposAdapter : BaseAdapter, IFilterable
	{
		private Context _context;
		private LayoutInflater _inflater;
		internal List<Models.Repository> _originalData;
		internal List<Models.Repository> _dataSource;

		public ReposAdapter(Context context) : this(context, new List<Models.Repository>())
		{

		}

		public ReposAdapter(Context context, List<Models.Repository> items)
		{
			_context = context;
			_dataSource = items;
			_inflater =  (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);


			Filter = new ByNameFilter(this);
		}

		public override int Count
		{
			get
			{
				return _dataSource.Count();
			}
		}

		public Models.Repository this[int i] 
		{
			get { return _dataSource[i]; }
		}

		public Filter Filter
		{
			get; private set;
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return  null ;
			}

		public override long GetItemId(int position)
		{
			return position;
		}

		public void Replace(List<Models.Repository> newRepos)
		{
			if (_originalData == null)
				_originalData = _dataSource;

			_dataSource.Clear();
			_dataSource.AddRange(newRepos);
			NotifyDataSetChanged();
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View rowView = _inflater.Inflate(Resource.Layout.ListItemRepo, parent, false);


			var titleTextView = rowView.FindViewById<TextView>(Resource.Id.RepoListTitle);
			var ownerTextView = rowView.FindViewById<TextView>(Resource.Id.RepoListOwner);
			var descriptionTextView = rowView.FindViewById<TextView>(Resource.Id.RepoListDescription);
			var languageTextView = rowView.FindViewById<TextView>(Resource.Id.RepoListLanguage);
			//var repoColor = rowView.FindViewById(Resource.Id.RepoListColor);

			titleTextView.Text = _dataSource[position].Name;
			if (System.String.IsNullOrEmpty(_dataSource[position].Description))
			{
				descriptionTextView.Visibility = ViewStates.Gone;
			}
			else
			{
				descriptionTextView.Visibility = ViewStates.Visible;
				descriptionTextView.Text = _dataSource[position].Description;
			}
			ownerTextView.Text = _dataSource[position].Owner;
			languageTextView.Text = _dataSource[position].Language;
			var color = Helpers.GetLanguageColor(_dataSource[position].Language);
			languageTextView.SetTextColor(Color.Argb(255, color[0], color[1], color[2]));

			return rowView;
		}
	}

	public class ByNameFilter : Filter
	{

		ReposAdapter _parent;
		public ByNameFilter(ReposAdapter parent)
		{
			_parent = parent;
		}

		protected override FilterResults PerformFiltering(ICharSequence constraint)
		{

			var returnObj = new FilterResults();
			var results = new List<Models.Repository>();
			if (_parent._originalData == null)
				_parent._originalData = _parent._dataSource;

			if (constraint == null) return returnObj;

			if (_parent._originalData != null && _parent._originalData.Any())
			{
				// Compare constraint to all names lowercased. 
				// It they are contained they are added to results.
				results.AddRange(
					_parent._originalData.Where(
						chemical => chemical.Name.ToLower().Contains(constraint.ToString())));
			}

			// Nasty piece of .NET to Java wrapping, be careful with this!
			returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
			returnObj.Count = results.Count;

			constraint.Dispose();

			return returnObj;
		}

		protected override void PublishResults(ICharSequence constraint, FilterResults results)
		{
			using (var values = results.Values)
				_parent._dataSource = values.ToArray<Object>()
					.Select(r => r.ToNetObject<Models.Repository>()).ToList();

			_parent.NotifyDataSetChanged();

			// Don't do this and see GREF counts rising
			constraint.Dispose();
			results.Dispose();
			
		}
	}
}
