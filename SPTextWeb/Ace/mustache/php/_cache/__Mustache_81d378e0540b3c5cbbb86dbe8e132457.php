<?php

class __Mustache_81d378e0540b3c5cbbb86dbe8e132457 extends Mustache_Template
{
    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $buffer = '';

        $buffer .= $indent . '<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="span12">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="widget-box">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-header widget-header-blue widget-header-flat wi1dget-header-large">';
        $buffer .= "\n";
        $buffer .= $indent . '				<h4 class="lighter">New Item Wizard</h4>';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="widget-toolbar">';
        $buffer .= "\n";
        $buffer .= $indent . '					<label>';
        $buffer .= "\n";
        $buffer .= $indent . '						<small class="green"><b>Validation</b></small>';
        $buffer .= "\n";
        $buffer .= $indent . '						<input id="skip-validation" type="checkbox" class="ace-switch ace-switch-4" /><span class="lbl"></span>';
        $buffer .= "\n";
        $buffer .= $indent . '					</label>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '			 <div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '				';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<div id="fuelux-wizard" class="row-fluid hidden">';
        $buffer .= "\n";
        $buffer .= $indent . '					  <ul class="wizard-steps">';
        $buffer .= "\n";
        $buffer .= $indent . '						<li data-target="#step1" class="active"><span class="step">1</span> <span class="title">Validation states</span></li>';
        $buffer .= "\n";
        $buffer .= $indent . '						<li data-target="#step2"><span class="step">2</span> <span class="title">Alerts</span></li>';
        $buffer .= "\n";
        $buffer .= $indent . '						<li data-target="#step3"><span class="step">3</span> <span class="title">Payment Info</span></li>';
        $buffer .= "\n";
        $buffer .= $indent . '						<li data-target="#step4"><span class="step">4</span> <span class="title">Other Info</span></li>';
        $buffer .= "\n";
        $buffer .= $indent . '					  </ul>';
        $buffer .= "\n";
        $buffer .= $indent . '					</div>';
        $buffer .= "\n";
        $buffer .= $indent . '					';
        $buffer .= "\n";
        $buffer .= $indent . '					<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '					';
        $buffer .= "\n";
        $buffer .= $indent . '					<div class="step-content row-fluid position-relative">';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="step-pane active" id="step1">';
        $buffer .= "\n";
        $buffer .= $indent . '							<h3 class="lighter block green">Enter the following information</h3>';
        $buffer .= "\n";
        $buffer .= $indent . '							';
        $buffer .= "\n";
        $buffer .= $indent . '							<form class="form-horizontal" id="sample-form">';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group warning">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="inputWarning">Input with warning</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="span6 input-icon input-icon-right">';
        $buffer .= "\n";
        $buffer .= $indent . '											<input class="span12" type="text" id="inputWarning" />';
        $buffer .= "\n";
        $buffer .= $indent . '											<i class="icon-warning-sign"></i>';
        $buffer .= "\n";
        $buffer .= $indent . '										</span>';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="help-inline">Something may have gone wrong</span>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group error">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="inputError">Error with tooltip</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="span6 input-icon input-icon-right">';
        $buffer .= "\n";
        $buffer .= $indent . '											<input class="tooltip-error span12" type="text" id="inputError" data-rel="tooltip" title="Error info!" data-trigger="focus" />';
        $buffer .= "\n";
        $buffer .= $indent . '											<i class="icon-remove-sign"></i>';
        $buffer .= "\n";
        $buffer .= $indent . '										</span>';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="help-inline">Please correct the error</span>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group success">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="inputSuccess">Input with success</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="span6 input-icon input-icon-right">';
        $buffer .= "\n";
        $buffer .= $indent . '											<input class="span12" type="text" id="inputSuccess" />';
        $buffer .= "\n";
        $buffer .= $indent . '											<i class="icon-ok-sign"></i>';
        $buffer .= "\n";
        $buffer .= $indent . '										</span>	';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="help-inline">Woohoo!</span>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group info">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="inputInfo">Blue Input with Info</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="span6 input-icon input-icon-right">';
        $buffer .= "\n";
        $buffer .= $indent . '											<input class="span12" type="text" id="inputInfo" />';
        $buffer .= "\n";
        $buffer .= $indent . '											<i class="icon-info-sign"></i>';
        $buffer .= "\n";
        $buffer .= $indent . '										</span>	';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="help-inline">Info tip help!</span>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '							</form>';
        $buffer .= "\n";
        $buffer .= $indent . '							';
        $buffer .= "\n";
        $buffer .= $indent . '							';
        $buffer .= "\n";
        $buffer .= $indent . '							';
        $buffer .= "\n";
        $buffer .= $indent . '							<form class="form-horizontal" id="validation-form" method="get">';
        $buffer .= "\n";
        $buffer .= $indent . '							  ';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="email">Email Address:</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<div class="span12">';
        $buffer .= "\n";
        $buffer .= $indent . '											<input type="email" name="email" id="email" class="span6" />';
        $buffer .= "\n";
        $buffer .= $indent . '										</div>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="password">Password:</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<div class="span12">';
        $buffer .= "\n";
        $buffer .= $indent . '											<input type="password" name="password" id="password" class="span4" />';
        $buffer .= "\n";
        $buffer .= $indent . '										</div>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="password2">Confirm Password:</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<div class="span12">';
        $buffer .= "\n";
        $buffer .= $indent . '											<input type="password" name="password2" id="password2" class="span4" />';
        $buffer .= "\n";
        $buffer .= $indent . '										</div>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="hr hr-dotted"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '								';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="name">Company Name:</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="span12">';
        $buffer .= "\n";
        $buffer .= $indent . '											<input class="span6" type="text" id="name" name="name" />';
        $buffer .= "\n";
        $buffer .= $indent . '										</span>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="phone">Phone Number:</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<div class="span3 input-prepend">';
        $buffer .= "\n";
        $buffer .= $indent . '											<span class="add-on"><i class="icon-phone"></i></span>';
        $buffer .= "\n";
        $buffer .= $indent . '											<input class="span12" type="tel" id="phone" name="phone" />';
        $buffer .= "\n";
        $buffer .= $indent . '										</div>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="url">Company URL:</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<div class="span12">';
        $buffer .= "\n";
        $buffer .= $indent . '											<input type="url" id="url" name="url" class="span8" />';
        $buffer .= "\n";
        $buffer .= $indent . '										</div>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="hr hr-dotted"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '								';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label">Subscribe to</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="span12">';
        $buffer .= "\n";
        $buffer .= $indent . '											<label><input name="subscription" value="1" type="checkbox" /><span class="lbl"> Latest news and announcements</span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '											<label><input name="subscription" value="2" type="checkbox" /><span class="lbl"> Product offers and discounts</span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '										</span>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label">Gender</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="span12">';
        $buffer .= "\n";
        $buffer .= $indent . '											<label class="blue"><input name="gender" value="1" type="radio" /><span class="lbl"> Male</span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '											<label class="blue"><input name="gender" value="2" type="radio" /><span class="lbl"> Female</span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '										</span>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="hr hr-dotted"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '								';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="state">State</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="span12">';
        $buffer .= "\n";
        $buffer .= $indent . '											<select id="state" name="state" class="chzn-select" data-placeholder="Click to Choose...">';
        $buffer .= "\n";
        $buffer .= $indent . '												<option value=""></option>';
        $buffer .= "\n";
        $buffer .= $indent . '												<option value="AL">Alabama</option><option value="AK">Alaska</option><option value="AZ">Arizona</option><option value="AR">Arkansas</option><option value="CA">California</option><option value="CO">Colorado</option><option value="CT">Connecticut</option><option value="DE">Delaware</option><option value="FL">Florida</option><option value="GA">Georgia</option><option value="HI">Hawaii</option><option value="ID">Idaho</option><option value="IL">Illinois</option><option value="IN">Indiana</option><option value="IA">Iowa</option><option value="KS">Kansas</option><option value="KY">Kentucky</option><option value="LA">Louisiana</option><option value="ME">Maine</option><option value="MD">Maryland</option><option value="MA">Massachusetts</option><option value="MI">Michigan</option><option value="MN">Minnesota</option><option value="MS">Mississippi</option><option value="MO">Missouri</option><option value="MT">Montana</option><option value="NE">Nebraska</option><option value="NV">Nevada</option><option value="NH">New Hampshire</option><option value="NJ">New Jersey</option><option value="NM">New Mexico</option><option value="NY">New York</option><option value="NC">North Carolina</option><option value="ND">North Dakota</option><option value="OH">Ohio</option><option value="OK">Oklahoma</option><option value="OR">Oregon</option><option value="PA">Pennsylvania</option><option value="RI">Rhode Island</option><option value="SC">South Carolina</option><option value="SD">South Dakota</option><option value="TN">Tennessee</option><option value="TX">Texas</option><option value="UT">Utah</option><option value="VT">Vermont</option><option value="VA">Virginia</option><option value="WA">Washington</option><option value="WV">West Virginia</option><option value="WI">Wisconsin</option><option value="WY">Wyoming</option>';
        $buffer .= "\n";
        $buffer .= $indent . '											</select>';
        $buffer .= "\n";
        $buffer .= $indent . '										</span>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="platform">Platform</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="span12">';
        $buffer .= "\n";
        $buffer .= $indent . '											<select class="span3" id="platform" name="platform">';
        $buffer .= "\n";
        $buffer .= $indent . '												<option value="">------------------</option>';
        $buffer .= "\n";
        $buffer .= $indent . '												<option value="linux">Linux</option>';
        $buffer .= "\n";
        $buffer .= $indent . '												<option value="windows">Windows</option>';
        $buffer .= "\n";
        $buffer .= $indent . '												<option value="mac">Mac OS</option>';
        $buffer .= "\n";
        $buffer .= $indent . '												<option value="ios">iOS</option>';
        $buffer .= "\n";
        $buffer .= $indent . '												<option value="android">Android</option>';
        $buffer .= "\n";
        $buffer .= $indent . '											</select>';
        $buffer .= "\n";
        $buffer .= $indent . '										</span>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<label class="control-label" for="comment">Comment</label>';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="span12">';
        $buffer .= "\n";
        $buffer .= $indent . '											<textarea class="span8" name="comment" id="comment"></textarea>';
        $buffer .= "\n";
        $buffer .= $indent . '										</span>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '								';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="control-group">';
        $buffer .= "\n";
        $buffer .= $indent . '									<div class="controls">';
        $buffer .= "\n";
        $buffer .= $indent . '										<span class="span6">';
        $buffer .= "\n";
        $buffer .= $indent . '											<label><input name="agree" id="agree" type="checkbox" /><span class="lbl"> I accept the policy</span></label>';
        $buffer .= "\n";
        $buffer .= $indent . '										</span>';
        $buffer .= "\n";
        $buffer .= $indent . '									</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '							  </form>';
        $buffer .= "\n";
        $buffer .= $indent . '							';
        $buffer .= "\n";
        $buffer .= $indent . '							';
        $buffer .= "\n";
        $buffer .= $indent . '							';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="step-pane" id="step2">';
        $buffer .= "\n";
        $buffer .= $indent . '							<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="alert alert-success">';
        $buffer .= "\n";
        $buffer .= $indent . '									<button type="button" class="close" data-dismiss="alert"><i class="icon-remove"></i></button>';
        $buffer .= "\n";
        $buffer .= $indent . '									<strong><i class="icon-ok"></i> Well done!</strong>';
        $buffer .= "\n";
        $buffer .= $indent . '									You successfully read this important alert message. <br />';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="alert alert-error">';
        $buffer .= "\n";
        $buffer .= $indent . '									<button type="button" class="close" data-dismiss="alert"><i class="icon-remove"></i></button>';
        $buffer .= "\n";
        $buffer .= $indent . '									<strong><i class="icon-remove"></i> Oh snap!</strong>';
        $buffer .= "\n";
        $buffer .= $indent . '									Change a few things up and try submitting again. <br />';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="alert alert-warning">';
        $buffer .= "\n";
        $buffer .= $indent . '									<button type="button" class="close" data-dismiss="alert"><i class="icon-remove"></i></button>';
        $buffer .= "\n";
        $buffer .= $indent . '									<strong>Warning!</strong>';
        $buffer .= "\n";
        $buffer .= $indent . '									Best check yo self, you\'re not looking too good. <br />';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '								<div class="alert alert-info">';
        $buffer .= "\n";
        $buffer .= $indent . '									<button type="button" class="close" data-dismiss="alert"><i class="icon-remove"></i></button>';
        $buffer .= "\n";
        $buffer .= $indent . '									<strong>Heads up!</strong>';
        $buffer .= "\n";
        $buffer .= $indent . '									This alert needs your attention, but it\'s not super important. <br />';
        $buffer .= "\n";
        $buffer .= $indent . '								</div>';
        $buffer .= "\n";
        $buffer .= $indent . '							</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="step-pane" id="step3">';
        $buffer .= "\n";
        $buffer .= $indent . '							<div class="center">';
        $buffer .= "\n";
        $buffer .= $indent . '								<h3 class="blue lighter">This is step 3</h3>';
        $buffer .= "\n";
        $buffer .= $indent . '							</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="step-pane" id="step4">';
        $buffer .= "\n";
        $buffer .= $indent . '							<div class="center">';
        $buffer .= "\n";
        $buffer .= $indent . '								<h3 class="green">Congrats!</h3>';
        $buffer .= "\n";
        $buffer .= $indent . '								Your product is ready to ship! Click finish to continue!';
        $buffer .= "\n";
        $buffer .= $indent . '							</div>';
        $buffer .= "\n";
        $buffer .= $indent . '						</div>';
        $buffer .= "\n";
        $buffer .= $indent . '					</div>';
        $buffer .= "\n";
        $buffer .= $indent . '					';
        $buffer .= "\n";
        $buffer .= $indent . '					<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '					';
        $buffer .= "\n";
        $buffer .= $indent . '					<div class="row-fluid wizard-actions">';
        $buffer .= "\n";
        $buffer .= $indent . '						<button class="btn btn-prev"><i class="icon-arrow-left"></i> Prev</button>';
        $buffer .= "\n";
        $buffer .= $indent . '						<button class="btn btn-success btn-next" data-last="Finish ">Next <i class="icon-arrow-right icon-on-right"></i></button>';
        $buffer .= "\n";
        $buffer .= $indent . '					</div>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '			 </div><!--/widget-main-->';
        $buffer .= "\n";
        $buffer .= $indent . '			</div><!--/widget-body-->';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '</div>';

        return $buffer;
    }
}
