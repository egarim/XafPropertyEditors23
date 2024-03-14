namespace XafPropertyEditors.Blazor.Server.Editors
{
    using DevExpress.Blazor.Primitives.Internal;
    using DevExpress.ExpressApp.Blazor.Components;
    using DevExpress.ExpressApp.Blazor.Components.Models;
    using DevExpress.ExpressApp.Blazor.Editors;
    using DevExpress.ExpressApp.Blazor.Editors.Adapters;
    using DevExpress.ExpressApp.Editors;
    using DevExpress.ExpressApp.Model;
    using DevExpress.ExpressApp.Utils;
    using Microsoft.AspNetCore.Components;

    [PropertyEditor(typeof(string), false)]
    public class CustomStringPropertyEditor : BlazorPropertyEditorBase
    {
        public CustomStringPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        protected override IComponentAdapter CreateComponentAdapter()
        {
            return new InputAdapter(new InputModel());
        }
        public InputModel ComponentModel => (Control as InputAdapter)?.ComponentModel;
    }
    public class InputAdapter : ComponentAdapterBase
    {
        public InputAdapter(InputModel componentModel)
        {
            ComponentModel = componentModel ?? throw new ArgumentNullException(nameof(componentModel));
            ComponentModel.ValueChanged = EventCallback.Factory.Create<string>(this, value => {
                componentModel.Value = value;
                RaiseValueChanged();
            });
        }
        public InputModel ComponentModel { get; }
        public override void SetAllowEdit(bool allowEdit) => ComponentModel.ReadOnly = !allowEdit;
        public override object GetValue() => ComponentModel.Value;
        public override void SetValue(object value) => ComponentModel.Value = (string)value;
        protected override RenderFragment CreateComponent() => ComponentModelObserver.Create(ComponentModel, InputRenderer.Create(ComponentModel));
        public override void SetAllowNull(bool allowNull) { /* ...*/ }
        public override void SetDisplayFormat(string displayFormat) { /* ...*/ }
        public override void SetEditMask(string editMask) { /* ...*/ }
        public override void SetEditMaskType(EditMaskType editMaskType) { /* ...*/ }
        public override void SetErrorIcon(ImageInfo errorIcon) { /* ...*/ }
        public override void SetErrorMessage(string errorMessage) { /* ...*/ }
        public override void SetIsPassword(bool isPassword) { /* ...*/ }
        public override void SetMaxLength(int maxLength) { /* ...*/ }
        public override void SetNullText(string nullText) { /* ...*/ }

      
    }
    public class InputModel : ComponentModelBase
    {
        public string Value
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }
        public EventCallback<string> ValueChanged
        {
            get => GetPropertyValue<EventCallback<string>>();
            set => SetPropertyValue(value);
        }
        public bool ReadOnly
        {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }
    }
}
