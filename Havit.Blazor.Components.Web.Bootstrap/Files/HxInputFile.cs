﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxInputFile : ComponentBase
	{
		/// <summary>
		/// URL of the server endpoint receiving the files.
		/// </summary>
		[Parameter] public string UploadUrl { get; set; }

		/// <summary>
		/// Gets or sets the event callback that will be invoked when the collection of selected files changes.
		/// </summary>
		[Parameter] public EventCallback<InputFileChangeEventArgs> OnChange { get; set; }

		/// <summary>
		/// Raised during running file upload (the frequency depends on browser implementation).
		/// </summary>
		[Parameter] public EventCallback<UploadProgressEventArgs> OnProgress { get; set; }

		/// <summary>
		/// Raised after a file is uploaded (for every single file separately).
		/// </summary>
		[Parameter] public EventCallback<FileUploadedEventArgs> OnFileUploaded { get; set; }

		/// <summary>
		/// Raised after a file is uploaded (for every single file separately).
		/// </summary>
		[Parameter] public EventCallback<UploadCompletedEventArgs> OnUploadCompleted { get; set; }

		/// <summary>
		/// Single <c>false</c> or multiple <c>true</c> files upload.
		/// </summary>
		[Parameter] public bool Multiple { get; set; }

		/// <summary>
		/// Label to render before input (or after input for Checkbox).		
		/// </summary>
		[Parameter] public string Label { get; set; }

		/// <summary>
		/// Label to render before input (or after input for Checkbox).
		/// </summary>
		[Parameter] public RenderFragment LabelTemplate { get; set; }

		/// <summary>
		/// Hint to render after input as form-text.
		/// </summary>
		[Parameter] public string Hint { get; set; }

		/// <summary>
		/// Hint to render after input as form-text.
		/// </summary>
		[Parameter] public RenderFragment HintTemplate { get; set; }

		/// <summary>
		/// Custom CSS class to render with wrapping div.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with the label.
		/// </summary>
		[Parameter] public string LabelCssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with the input element.
		/// </summary>
		[Parameter] public string InputCssClass { get; set; }

		[Parameter] public bool? Enabled { get; set; }

		public int FileCount => hxInputFileCoreComponentReference.FileCount;

		/// <summary>
		/// ID if the input element. Autogenerated when used with label.
		/// </summary>
		protected string InputId { get; private set; } = "hx" + Guid.NewGuid().ToString("N");

		/// <summary>
		/// CSS class to be rendered with the wrapping div.
		/// </summary>
		private protected virtual string CoreCssClass => "";

		/// <summary>
		/// CSS class to be rendered with the input element.
		/// </summary>
		private protected virtual string CoreInputCssClass => "form-control";

		/// <summary>
		/// CSS class to be rendered with the label.
		/// </summary>
		private protected virtual string CoreLabelCssClass => "form-label";

		/// <summary>
		/// CSS class to be rendered with the hint.
		/// </summary>
		private protected virtual string CoreHintCssClass => "form-text";

		private protected HxInputFileCore hxInputFileCoreComponentReference;

		/// <summary>
		/// Gets list of files chosen.
		/// </summary>
		public Task<FileInfo[]> GetFilesAsync() => hxInputFileCoreComponentReference.GetFilesAsync();

		/// <summary>
		/// Clears associated input-file element and resets component to initial state.
		/// </summary>
		public Task ResetAsync() => hxInputFileCoreComponentReference.ResetAsync();

		/// <summary>
		/// Starts the upload.
		/// </summary>
		/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
		/// <remarks>
		/// We do not want to make the Havit.Blazor library dependant on WebAssembly libraries (IAccessTokenProvider and such). Therefor the accessToken here...
		/// </remarks>
		public Task StartUploadAsync(string accessToken = null) => hxInputFileCoreComponentReference?.StartUploadAsync(accessToken);

		/// <summary>
		/// Uploads the file(s).
		/// </summary>
		/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
		public Task<UploadCompletedEventArgs> UploadAsync(string accessToken = null) => hxInputFileCoreComponentReference?.UploadAsync(accessToken);

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// no base call

			string cssClass = CssClassHelper.Combine(CoreCssClass, CssClass);

			// pokud nemáme css class, label, ani hint, budeme renderovat jako čistý input
			bool renderDiv = !String.IsNullOrEmpty(cssClass)
				|| !String.IsNullOrEmpty(Label)
				|| (LabelTemplate != null)
				|| !String.IsNullOrEmpty(Hint)
				|| (HintTemplate != null);

			if (renderDiv)
			{
				builder.OpenElement(1, "div");
				if (!String.IsNullOrEmpty(cssClass))
				{
					builder.AddAttribute(2, "class", cssClass);
				}
			}

			builder.OpenRegion(3);
			BuildRenderLabel(builder);
			builder.CloseRegion();

			builder.OpenRegion(4);
			BuildRenderHxInputFileCore(builder);
			builder.CloseRegion();

			builder.OpenRegion(9);
			BuildRenderHint(builder);
			builder.CloseRegion();

			if (renderDiv)
			{
				builder.CloseElement();
			}
		}

		/// <summary>
		/// Renders label when properties set.
		/// </summary>
		protected virtual void BuildRenderLabel(RenderTreeBuilder builder)
		{
			//  <label for="formGroupExampleInput">Example label</label>
			if (!String.IsNullOrEmpty(Label) || (LabelTemplate != null))
			{
				builder.OpenElement(1, "label");
				builder.AddAttribute(2, "for", InputId);
				builder.AddAttribute(3, "class", CssClassHelper.Combine(CoreLabelCssClass, LabelCssClass));
				builder.AddEventStopPropagationAttribute(4, "onclick", true);
				if (LabelTemplate == null)
				{
					builder.AddContent(5, Label);
				}
				builder.AddContent(6, LabelTemplate);
				builder.CloseElement();
			}
		}

		/// <summary>
		/// Render input. Enables to use some wrapping html, used for input-group in descenant.
		/// </summary>
		protected virtual void BuildRenderInputDecorated(RenderTreeBuilder builder)
		{
			// breaks the rule - ancesor is designed for descenant
			BuildRenderHxInputFileCore(builder);
		}

		/// <summary>
		/// Renders input.
		/// </summary>
		protected virtual void BuildRenderHxInputFileCore(RenderTreeBuilder builder)
		{
			builder.OpenComponent<HxInputFileCore>(1);
			builder.AddAttribute(1002, nameof(HxInputFileCore.Id), this.InputId);
			builder.AddAttribute(1003, nameof(HxInputFileCore.UploadUrl), this.UploadUrl);
			builder.AddAttribute(1004, nameof(HxInputFileCore.Multiple), this.Multiple);
			builder.AddAttribute(1005, nameof(HxInputFileCore.OnChange), this.OnChange);
			builder.AddAttribute(1006, nameof(HxInputFileCore.OnProgress), this.OnProgress);
			builder.AddAttribute(1007, nameof(HxInputFileCore.OnFileUploaded), this.OnFileUploaded);
			builder.AddAttribute(1008, nameof(HxInputFileCore.OnUploadCompleted), this.OnUploadCompleted);
			builder.AddAttribute(1009, "class", CssClassHelper.Combine(this.CoreInputCssClass, this.InputCssClass));
			builder.AddComponentReferenceCapture(1001, r => hxInputFileCoreComponentReference = (HxInputFileCore)r);
			builder.CloseComponent();
		}

		/// <summary>
		/// Renders hint when property HintTemplate set.
		/// </summary>
		protected virtual void BuildRenderHint(RenderTreeBuilder builder)
		{
			if (!String.IsNullOrEmpty(Hint) || (HintTemplate != null))
			{
				builder.OpenElement(1, "div");
				builder.AddAttribute(2, "class", CoreHintCssClass);
				builder.AddContent(3, Hint);
				builder.AddContent(4, HintTemplate);
				builder.CloseElement();
			}
		}

	}
}
