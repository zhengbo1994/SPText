<?php

class __Mustache_be7d1d774f32f45bc7096d3d5befdecc extends Mustache_Template
{
    private $lambdaHelper;

    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $this->lambdaHelper = new Mustache_LambdaHelper($this->mustache, $context);
        $buffer = '';

        $buffer .= $indent . '<form class="form-horizontal">';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '		<label class="control-label" for="form-field-1">Text Field</label>';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '			<input type="text" id="form-field-1" placeholder="Username">';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '		<label class="control-label" for="form-field-2">Password Field</label>';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '			<input type="password" id="form-field-2" placeholder="Password">';
        $buffer .= "\n";
        $buffer .= $indent . '			<span class="help-inline">Inline help text</span>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '		<label class="control-label" for="form-input-readonly">Readonly field</label>';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '			<input readonly type="text" id="form-input-readonly" value="This text field is readonly!" />';
        $buffer .= "\n";
        $buffer .= $indent . '			&nbsp; &nbsp;';
        $buffer .= "\n";
        $buffer .= $indent . '			<input type="checkbox" id="id-disable-check" />';
        $buffer .= "\n";
        $buffer .= $indent . '			<label class="lbl" for="id-disable-check"> Disable it!</label>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '		<label class="control-label" for="form-field-4">Relative Sizing</label>';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '			<input class="input-mini" type="text" id="form-field-4" placeholder=".input-mini">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="help-block" id="input-size-slider"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '		<label class="control-label" for="form-field-5">Grid Sizing</label>';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '			<input class="span1" type="text" id="form-field-5" placeholder=".span1">';
        $buffer .= "\n";
        $buffer .= $indent . '			<input class="span11" type="text" placeholder=".span11">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="help-block" id="input-span-slider"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '		<label class="control-label">Input with Icon</label>';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '			<span class="input-icon">';
        $buffer .= "\n";
        $buffer .= $indent . '				<input type="text" id="form-field-icon-1">';
        $buffer .= "\n";
        $buffer .= $indent . '				<i class="icon-leaf"></i>';
        $buffer .= "\n";
        $buffer .= $indent . '			</span>';
        $buffer .= "\n";
        $buffer .= $indent . '			';
        $buffer .= "\n";
        $buffer .= $indent . '			<span class="input-icon input-icon-right">';
        $buffer .= "\n";
        $buffer .= $indent . '				<input type="text" id="form-field-icon-2">';
        $buffer .= "\n";
        $buffer .= $indent . '				<i class="icon-leaf"></i>';
        $buffer .= "\n";
        $buffer .= $indent . '			</span>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '		<label class="control-label" for="form-field-6">Tooltip and help button</label>';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '			<input class="ace-tooltip" type="text" id="form-field-6" placeholder="Tooltip on hover" title="Hello Tooltip!" data-placement="bottom">';
        $buffer .= "\n";
        $buffer .= $indent . '			<span class="help-button ace-popover" data-trigger="hover" data-placement="left" data-content="More details." title="Popover on hover">?</span>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="form-actions">';
        $buffer .= "\n";
        $buffer .= $indent . '		<button class="btn btn-info" type="submit"><i class="icon-ok bigger-110"></i> Submit</button>';
        $buffer .= "\n";
        $buffer .= $indent . '		&nbsp; &nbsp; &nbsp;';
        $buffer .= "\n";
        $buffer .= $indent . '		<button class="btn" type="reset"><i class="icon-undo bigger-110"></i> Reset</button>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="hr"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '	  <div class="span4">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '				<h4>Text Area</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '				<span class="widget-toolbar">';
        $buffer .= "\n";
        $buffer .= $indent . '					<a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '					<a href="#" data-action="close"><i class="icon-remove"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '				</span>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '			 <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="form-field-8">Default</label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<textarea class="span12" id="form-field-8" placeholder="Default Text"></textarea>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="form-field-9">With Character Limit</label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<textarea class="span12 limited" id="form-field-9" data-maxlength="50"></textarea>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="form-field-11">Autosize</label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<textarea id="form-field-11" class="autosize-transition span12"></textarea>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	  </div><!--/span-->';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	  <div class="span4">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '				<h4>Masked Input</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '				<span class="widget-toolbar">';
        $buffer .= "\n";
        $buffer .= $indent . '					<a href="#" data-action="settings"><i class="icon-cog"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '					<a href="#" data-action="reload"><i class="icon-refresh"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '					<a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '					<a href="#" data-action="close"><i class="icon-remove"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '				</span>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '			 <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="form-field-mask-1">Date <small class="text-success">99/99/9999</small></label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<div class="input-append">';
        $buffer .= "\n";
        $buffer .= $indent . '						<input class="input-small input-mask-date" type="text" id="form-field-mask-1" />';
        $buffer .= "\n";
        $buffer .= $indent . '						<span class="btn btn-small"><i class="icon-calendar bigger-110"></i> Go!</span>';
        $buffer .= "\n";
        $buffer .= $indent . '					</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="form-field-mask-2">Phone <small class="text-warning">(999) 999-9999</small></label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<div class="input-prepend">';
        $buffer .= "\n";
        $buffer .= $indent . '						<span class="add-on"><i class="icon-phone"></i></span>';
        $buffer .= "\n";
        $buffer .= $indent . '						<input class="input-medium input-mask-phone" type="text" id="form-field-mask-2" />';
        $buffer .= "\n";
        $buffer .= $indent . '					</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="form-field-mask-3">Product Key <small class="text-error">a*-999-a999</small></label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<div class="input-prepend">';
        $buffer .= "\n";
        $buffer .= $indent . '						<span class="add-on"><i class="icon-key"></i></span>';
        $buffer .= "\n";
        $buffer .= $indent . '						<input class="input-medium input-mask-product" type="text" id="form-field-mask-3" />';
        $buffer .= "\n";
        $buffer .= $indent . '					</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="form-field-mask-4">Eye Script <small class="text-info">~9.99 ~9.99 999</small></label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<div>';
        $buffer .= "\n";
        $buffer .= $indent . '						<input class="input-medium input-mask-eyescript" type="text" id="form-field-mask-4" />';
        $buffer .= "\n";
        $buffer .= $indent . '					</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '			 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	  </div><!--/span-->';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	  <div class="span4">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '				<h4>Select Box</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '				<span class="widget-toolbar">';
        $buffer .= "\n";
        $buffer .= $indent . '					<a href="#" data-action="settings"><i class="icon-cog"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '					<a href="#" data-action="reload"><i class="icon-refresh"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '					<a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '					<a href="#" data-action="close"><i class="icon-remove"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '				</span>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '			 <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="form-field-select-1">Default</label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<select id="form-field-select-1">';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value=""></option>';
        $buffer .= "\n";
        // 'page.states' section
        $buffer .= $this->sectionBad5c6996822ab0a7ef5abef84a13847($context, $indent, $context->findDot('page.states'));
        $buffer .= $indent . '					</select>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="form-field-select-2">Multiple</label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<select id="form-field-select-2" multiple="multiple">';
        $buffer .= "\n";
        // 'page.states' section
        $buffer .= $this->sectionDaed8b0a2479f413c3876988777b909f($context, $indent, $context->findDot('page.states'));
        $buffer .= $indent . '					</select>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="form-field-select-3">Chosen</label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<select class="chzn-select" id="form-field-select-3" data-placeholder="Choose a Country...">';
        $buffer .= "\n";
        $buffer .= $indent . '						 <option value=""></option>';
        $buffer .= "\n";
        // 'page.states' section
        $buffer .= $this->sectionF877aa7a1eedbac8a9093ea1e7bd2c70($context, $indent, $context->findDot('page.states'));
        $buffer .= $indent . '					</select>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="form-field-select-4">Chosen Multiple</label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<select multiple class="chzn-select" id="form-field-select-4" data-placeholder="Choose a Country...">';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value=""></option>';
        $buffer .= "\n";
        // 'page.states' section
        $buffer .= $this->sectionB9571f13fc6ecd3113cf2e73ca385717($context, $indent, $context->findDot('page.states'));
        $buffer .= $indent . '					</select>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	  </div><!--/span-->';
        $buffer .= "\n";
        $buffer .= $indent . '		  ';
        $buffer .= "\n";
        $buffer .= $indent . '	</div><!--/row-->';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="space-24"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<h3 class="header smaller lighter blue">Checkboxes & Radio <small>All Checkboxes, Radios and Switch Buttons Are Pure CSS</small></h3>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '	  <div class="span5">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '			<label class="control-label">Checkbox</label>';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '				<label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<input name="form-field-checkbox" type="checkbox" /><span class="lbl"> choice 1</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				</label>';
        $buffer .= "\n";
        $buffer .= $indent . '				<label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<input name="form-field-checkbox" type="checkbox" /><span class="lbl">  choice 2</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				</label>';
        $buffer .= "\n";
        $buffer .= $indent . '				<label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<input name="form-field-checkbox" class="ace-checkbox-2" type="checkbox" /><span class="lbl"> choice 3</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				</label>';
        $buffer .= "\n";
        $buffer .= $indent . '				<label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<input name="form-field-checkbox" disabled type="checkbox" /><span class="lbl"> disabled</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				</label>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	  <div class="span6">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '			<label class="control-label">Radio</label>';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '				<label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<input name="form-field-radio" type="radio" /><span class="lbl"> radio option 1</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				</label>';
        $buffer .= "\n";
        $buffer .= $indent . '			';
        $buffer .= "\n";
        $buffer .= $indent . '				<label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<input name="form-field-radio" type="radio" /><span class="lbl"> radio option 2</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				</label>';
        $buffer .= "\n";
        $buffer .= $indent . '			';
        $buffer .= "\n";
        $buffer .= $indent . '				<label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<input name="form-field-radio" type="radio" /><span class="lbl"> radio option 3</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				</label>';
        $buffer .= "\n";
        $buffer .= $indent . '				';
        $buffer .= "\n";
        $buffer .= $indent . '				<label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<input disabled name="form-field-radio" type="radio" /><span class="lbl"> disabled</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				</label>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div><!--/row-->';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '		<label class="control-label">On/Off Switches</label>';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="span3">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label><input name="switch-field-1" class="ace-switch" type="checkbox" /><span class="lbl"></span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="span3">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label><input name="switch-field-1" class="ace-switch ace-switch-2" type="checkbox" /><span class="lbl"></span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="span3">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label><input name="switch-field-1" class="ace-switch ace-switch-3" type="checkbox" /><span class="lbl"></span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="span3">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label><input name="switch-field-1" class="ace-switch ace-switch-4" type="checkbox" /><span class="lbl"></span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="span3">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label><input name="switch-field-1" class="ace-switch ace-switch-5" type="checkbox" /><span class="lbl"></span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="span3">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label><input name="switch-field-1" class="ace-switch ace-switch-6" type="checkbox" /><span class="lbl"></span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="span3">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label><input name="switch-field-1" class="ace-switch ace-switch-7" type="checkbox" /><span class="lbl"></span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="span4">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '					<h4>Custom File Input</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '					<span class="widget-toolbar">';
        $buffer .= "\n";
        $buffer .= $indent . '						<a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '						<a href="#" data-action="close"><i class="icon-remove"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '					</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '				 <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '					<input type="file" id="id-input-file-1" />';
        $buffer .= "\n";
        $buffer .= $indent . '					<input type="file" id="id-input-file-2" />';
        $buffer .= "\n";
        $buffer .= $indent . '					<input multiple type="file" id="id-input-file-3" />';
        $buffer .= "\n";
        $buffer .= $indent . '				 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="span4">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '					<h4>jQuery UI Sliders</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '				 <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '					<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="span1">';
        $buffer .= "\n";
        $buffer .= $indent . '							<div id="slider-range"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="span11">';
        $buffer .= "\n";
        $buffer .= $indent . '							<div id="eq">';
        $buffer .= "\n";
        $buffer .= $indent . '								<span class="ui-slider-green">77</span>';
        $buffer .= "\n";
        $buffer .= $indent . '								<span class="ui-slider-red">55</span>';
        $buffer .= "\n";
        $buffer .= $indent . '								<span class="ui-slider-purple">33</span>';
        $buffer .= "\n";
        $buffer .= $indent . '								<span class="ui-slider-orange">40</span>';
        $buffer .= "\n";
        $buffer .= $indent . '								<span class="ui-slider-dark">88</span>';
        $buffer .= "\n";
        $buffer .= $indent . '							</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '					</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="span4">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '					<h4>Spinners</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '				 <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '					<input type="text" class="input-mini" id="spinner1" />';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="space-6"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '					<input type="text" class="input-mini" id="spinner2" />';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="space-6"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '					<input type="text" class="input-mini" id="spinner3" />';
        $buffer .= "\n";
        $buffer .= $indent . '				 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="span4">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '					<h4>Date Picker</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '					<span class="widget-toolbar">';
        $buffer .= "\n";
        $buffer .= $indent . '						<a href="#" data-action="settings"><i class="icon-cog"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '						<a href="#" data-action="reload"><i class="icon-refresh"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '						<a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '						<a href="#" data-action="close"><i class="icon-remove"></i></a>';
        $buffer .= "\n";
        $buffer .= $indent . '					</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '				 <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '							<label for="id-date-picker-1">Date Picker</label>';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '							<div class="row-fluid input-append date">';
        $buffer .= "\n";
        $buffer .= $indent . '								<input class="span10 date-picker" id="id-date-picker-1" type="text" data-date-format="dd-mm-yyyy" />';
        $buffer .= "\n";
        $buffer .= $indent . '								<span class="add-on"><i class="icon-calendar"></i></span>';
        $buffer .= "\n";
        $buffer .= $indent . '							</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						';
        $buffer .= "\n";
        $buffer .= $indent . '						<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '						';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '							<label for="id-date-range-picker-1">Date Range Picker</label>';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '							<div class="input-prepend">';
        $buffer .= "\n";
        $buffer .= $indent . '								<span class="add-on"><i class="icon-calendar"></i></span>';
        $buffer .= "\n";
        $buffer .= $indent . '								<input class="span11" type="text" name="date-range-picker" id="id-date-range-picker-1" />';
        $buffer .= "\n";
        $buffer .= $indent . '							</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						';
        $buffer .= "\n";
        $buffer .= $indent . '						<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '						';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '							<label for="timepicker1">Time Picker</label>';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '							<div class="input-append bootstrap-timepicker">';
        $buffer .= "\n";
        $buffer .= $indent . '								<input id="timepicker1" type="text" class="input-small" />';
        $buffer .= "\n";
        $buffer .= $indent . '								<span class="add-on"><i class="icon-time"></i></span>';
        $buffer .= "\n";
        $buffer .= $indent . '							</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '				 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="span4">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '					<h4><i class="icon-tint"></i> Color Picker</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '				 <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '				';
        $buffer .= "\n";
        $buffer .= $indent . '					<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '						<label for="colorpicker1">Color Picker</label>';
        $buffer .= "\n";
        $buffer .= $indent . '					</div>';
        $buffer .= "\n";
        $buffer .= $indent . '					<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="bootstrap-colorpicker">';
        $buffer .= "\n";
        $buffer .= $indent . '							<input id="colorpicker1" type="text" class="input-mini" />							';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '					</div>';
        $buffer .= "\n";
        $buffer .= $indent . '					';
        $buffer .= "\n";
        $buffer .= $indent . '					<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '					<label for="simple-colorpicker-1">Custom Color Picker</label>';
        $buffer .= "\n";
        $buffer .= $indent . '					<select id="simple-colorpicker-1" class="hidden">';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#ac725e">#ac725e</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#d06b64">#d06b64</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#f83a22">#f83a22</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#fa573c">#fa573c</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#ff7537">#ff7537</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#ffad46" selected>#ffad46</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#42d692">#42d692</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#16a765">#16a765</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#7bd148">#7bd148</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#b3dc6c">#b3dc6c</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#fbe983">#fbe983</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#fad165">#fad165</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#92e1c0">#92e1c0</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#9fe1e7">#9fe1e7</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#9fc6e7">#9fc6e7</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#4986e7">#4986e7</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#9a9cff">#9a9cff</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#b99aff">#b99aff</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#c2c2c2">#c2c2c2</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#cabdbf">#cabdbf</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#cca6ac">#cca6ac</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#f691b2">#f691b2</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#cd74e6">#cd74e6</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#a47ae2">#a47ae2</option>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <option value="#555">#555</option>';
        $buffer .= "\n";
        $buffer .= $indent . '					</select>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '				 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="span4">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '					<h4><i class="icon-dashboard"></i> Knob Input</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '				 <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '						';
        $buffer .= "\n";
        $buffer .= $indent . '						  <div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '							<div class="span6">';
        $buffer .= "\n";
        $buffer .= $indent . '								<input type="text" class="input-small knob" value="15" data-min="0" data-max="100" data-step="10" data-width="80" data-height="80" data-thickness=".2" />';
        $buffer .= "\n";
        $buffer .= $indent . '							</div>';
        $buffer .= "\n";
        $buffer .= $indent . '							<div class="span6">';
        $buffer .= "\n";
        $buffer .= $indent . '								<input type="text" class="input-small knob" value="41" data-min="0" data-max="100" data-width="80" data-height="80" data-thickness=".2" data-fgColor="#87B87F" data-displayPrevious=true data-angleArc=250 data-angleOffset=-125 />';
        $buffer .= "\n";
        $buffer .= $indent . '							</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '						  <div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '							<div class="span12 center">';
        $buffer .= "\n";
        $buffer .= $indent . '								<input type="text" class="input-small knob" data-min="0" data-max="10"  data-width="150" data-height="150" data-thickness=".2" data-fgColor="#B8877F" data-angleOffset=90 data-cursor="true" />';
        $buffer .= "\n";
        $buffer .= $indent . '							</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '						';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '</form>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '<div class="space-24"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '<h3 class="header smaller lighter green">Form Layouts</h3>';
        $buffer .= "\n";
        $buffer .= $indent . '<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="span5">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '		  <div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '			<h4>Default</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '		  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '		  <div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '		   <div class="widget-main no-padding">';
        $buffer .= "\n";
        $buffer .= $indent . '			<form>';
        $buffer .= "\n";
        $buffer .= $indent . '			  <!-- <legend>Form</legend> -->';
        $buffer .= "\n";
        $buffer .= $indent . '			  <fieldset>';
        $buffer .= "\n";
        $buffer .= $indent . '				<label>Label name</label>';
        $buffer .= "\n";
        $buffer .= $indent . '				<input type="text" placeholder="Type something&hellip;">';
        $buffer .= "\n";
        $buffer .= $indent . '				<span class="help-block">Example block-level help text here.</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				<label class="pull-right">';
        $buffer .= "\n";
        $buffer .= $indent . '				  <input type="checkbox"><span class="lbl"> check me out</span>';
        $buffer .= "\n";
        $buffer .= $indent . '				</label>';
        $buffer .= "\n";
        $buffer .= $indent . '			  </fieldset>';
        $buffer .= "\n";
        $buffer .= $indent . '			  <div class="form-actions center">';
        $buffer .= "\n";
        $buffer .= $indent . '				<button onclick="return false;" class="btn btn-small btn-success">Submit <i class="icon-arrow-right icon-on-right bigger-110"></i></button>';
        $buffer .= "\n";
        $buffer .= $indent . '			  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</form>';
        $buffer .= "\n";
        $buffer .= $indent . '		   </div>';
        $buffer .= "\n";
        $buffer .= $indent . '		  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="span7">';
        $buffer .= "\n";
        $buffer .= $indent . '	  <div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '		  <div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '			<h4>Inline Forms</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '		  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '		  <div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '		   <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '			<form class="form-inline">';
        $buffer .= "\n";
        $buffer .= $indent . '			<input type="text" class="input-small" placeholder="Email">';
        $buffer .= "\n";
        $buffer .= $indent . '			<input type="password" class="input-small" placeholder="Password">';
        $buffer .= "\n";
        $buffer .= $indent . '			<label class="checkbox">';
        $buffer .= "\n";
        $buffer .= $indent . '				<input type="checkbox"><span class="lbl"> remember me</span>';
        $buffer .= "\n";
        $buffer .= $indent . '			</label>';
        $buffer .= "\n";
        $buffer .= $indent . '			<button onclick="return false;" class="btn btn-info btn-small"><i class="icon-key bigger-110"></i>Login</button>';
        $buffer .= "\n";
        $buffer .= $indent . '			</form>';
        $buffer .= "\n";
        $buffer .= $indent . '		   </div>';
        $buffer .= "\n";
        $buffer .= $indent . '		  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '	  ';
        $buffer .= "\n";
        $buffer .= $indent . '	  <div class="space-6"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '	  ';
        $buffer .= "\n";
        $buffer .= $indent . '	  <div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '		  <div class="widget-header widget-header-small">';
        $buffer .= "\n";
        $buffer .= $indent . '			<h5 class="lighter">Search Form</h5>';
        $buffer .= "\n";
        $buffer .= $indent . '		  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '		  <div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '		   <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '			<form class="form-search">';
        $buffer .= "\n";
        $buffer .= $indent . '			<input type="text" class="input-medium search-query">';
        $buffer .= "\n";
        $buffer .= $indent . '			<button onclick="return false;" class="btn btn-purple btn-small">Search <i class="icon-search icon-on-right bigger-110"></i></button>';
        $buffer .= "\n";
        $buffer .= $indent . '			</form>';
        $buffer .= "\n";
        $buffer .= $indent . '		   </div>';
        $buffer .= "\n";
        $buffer .= $indent . '		  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	  </div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '</div>';

        return $buffer;
    }

    private function sectionBad5c6996822ab0a7ef5abef84a13847(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
							<option value=\'{{abbr}}\'>{{name}}</option>
						  ';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '							<option value=\'';
                $value = $this->resolveValue($context->find('abbr'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '\'>';
                $value = $this->resolveValue($context->find('name'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</option>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionDaed8b0a2479f413c3876988777b909f(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
						  <option value=\'{{abbr}}\'>{{name}}</option>
						';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '						  <option value=\'';
                $value = $this->resolveValue($context->find('abbr'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '\'>';
                $value = $this->resolveValue($context->find('name'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</option>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionF877aa7a1eedbac8a9093ea1e7bd2c70(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
						  <option value=\'{{abbr}}\'>{{name}}</option>
						 ';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '						  <option value=\'';
                $value = $this->resolveValue($context->find('abbr'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '\'>';
                $value = $this->resolveValue($context->find('name'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</option>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionB9571f13fc6ecd3113cf2e73ca385717(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
						  <option value=\'{{abbr}}\'>{{name}}</option>
						  ';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '						  <option value=\'';
                $value = $this->resolveValue($context->find('abbr'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '\'>';
                $value = $this->resolveValue($context->find('name'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</option>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }
}
