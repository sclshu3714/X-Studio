using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using XStudio.App.Models.Enums;

namespace XStudio.App.ViewModel
{
    public class FieldTemplateSelector : DataTemplateSelector {
        public DataTemplate? TextTemplate { get; set; }
        public DataTemplate? EmailTemplate { get; set; }
        public DataTemplate? NumberTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container) {
            if (item is FormFieldViewModel field) {
                switch (field.FieldType) {
                    case FormFieldType.Text:
                        return TextTemplate;
                    case FormFieldType.Email:
                        return EmailTemplate;
                    case FormFieldType.Number:
                        return NumberTemplate;
                    default:
                        return base.SelectTemplate(item, container);
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
