using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using XafPropertyEditors.Module.BusinessObjects;
using System.Collections;

namespace XafPropertyEditors.Blazor.Server.Editors
{
    public class PictureItemListViewModel : ComponentModelBase
    {
        public IEnumerable<IPictureItem> Data
        {
            get => GetPropertyValue<IEnumerable<IPictureItem>>();
            set => SetPropertyValue(value);
        }
        public void Refresh() => RaiseChanged();
        public void OnItemClick(IPictureItem item) =>
            ItemClick?.Invoke(this, new PictureItemListViewModelItemClickEventArgs(item));
        public event EventHandler<PictureItemListViewModelItemClickEventArgs> ItemClick;
    }
    public class PictureItemListViewModelItemClickEventArgs : EventArgs
    {
        public PictureItemListViewModelItemClickEventArgs(IPictureItem item)
        {
            Item = item;
        }
        public IPictureItem Item { get; }
    }

    [ListEditor(typeof(IPictureItem))]
    public class BlazorCustomListEditor : ListEditor
    {
        public class PictureItemListViewHolder : IComponentContentHolder
        {
            private RenderFragment componentContent;
            public PictureItemListViewHolder(PictureItemListViewModel componentModel)
            {
                ComponentModel =
                    componentModel ?? throw new ArgumentNullException(nameof(componentModel));
            }
            private RenderFragment CreateComponent() =>
                ComponentModelObserver.Create(ComponentModel,
                                                PictureItemListViewRenderer.Create(ComponentModel));
            public PictureItemListViewModel ComponentModel { get; }
            RenderFragment IComponentContentHolder.ComponentContent =>
                componentContent ??= CreateComponent();
        }
        private IPictureItem[] selectedObjects = Array.Empty<IPictureItem>();
        public BlazorCustomListEditor(IModelListView model) : base(model) { }
        protected override object CreateControlsCore() =>
            new PictureItemListViewHolder(new PictureItemListViewModel());
        protected override void AssignDataSourceToControl(object dataSource)
        {
            if (Control is PictureItemListViewHolder holder)
            {
                if (holder.ComponentModel.Data is IBindingList bindingList)
                {
                    bindingList.ListChanged -= BindingList_ListChanged;
                }
                holder.ComponentModel.Data =
                    (dataSource as IEnumerable)?.OfType<IPictureItem>().OrderBy(i => i.Text);
                if (dataSource is IBindingList newBindingList)
                {
                    newBindingList.ListChanged += BindingList_ListChanged;
                }
            }
        }
        protected override void OnControlsCreated()
        {
            if (Control is PictureItemListViewHolder holder)
            {
                holder.ComponentModel.ItemClick += ComponentModel_ItemClick;
            }
            base.OnControlsCreated();
        }
        public override void BreakLinksToControls()
        {
            if (Control is PictureItemListViewHolder holder)
            {
                holder.ComponentModel.ItemClick -= ComponentModel_ItemClick;
            }
            AssignDataSourceToControl(null);
            base.BreakLinksToControls();
        }
        public override void Refresh()
        {
            if (Control is PictureItemListViewHolder holder)
            {
                holder.ComponentModel.Refresh();
            }
        }
        private void BindingList_ListChanged(object sender, ListChangedEventArgs e)
        {
            Refresh();
        }
        private void ComponentModel_ItemClick(object sender,
                                                PictureItemListViewModelItemClickEventArgs e)
        {
            selectedObjects = new IPictureItem[] { e.Item };
            OnSelectionChanged();
            OnProcessSelectedItem();
        }
        public override SelectionType SelectionType => SelectionType.Full;
        public override IList GetSelectedObjects() => selectedObjects;
    }
}
