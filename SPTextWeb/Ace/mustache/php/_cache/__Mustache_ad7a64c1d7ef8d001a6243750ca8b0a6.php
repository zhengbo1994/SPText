<?php

class __Mustache_ad7a64c1d7ef8d001a6243750ca8b0a6 extends Mustache_Template
{
    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $buffer = '';

        $buffer .= $indent . '<li class="light-blue user-profile">';
        $buffer .= "\n";
        $buffer .= $indent . '	<a data-toggle="dropdown" href="#" class="user-menu dropdown-toggle">';
        $buffer .= "\n";
        $buffer .= $indent . '		<img class="nav-user-photo" src="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '/avatars/user.jpg" alt="Jason\'s Photo">';
        $buffer .= "\n";
        $buffer .= $indent . '		<span id="user_info">';
        $buffer .= "\n";
        $buffer .= $indent . '			<small>Welcome,</small> Jason';
        $buffer .= "\n";
        $buffer .= $indent . '		</span>';
        $buffer .= "\n";
        $buffer .= $indent . '		<i class="icon-caret-down"></i>';
        $buffer .= "\n";
        $buffer .= $indent . '	</a>';
        $buffer .= "\n";
        $buffer .= $indent . '	<ul class="pull-right dropdown-menu dropdown-yellow dropdown-caret dropdown-closer" id="user_menu">';
        $buffer .= "\n";
        $buffer .= $indent . '		<li><a href="#"><i class="icon-cog"></i> Settings</a></li>';
        $buffer .= "\n";
        $buffer .= $indent . '		<li><a href="#"><i class="icon-user"></i> Profile</a></li>';
        $buffer .= "\n";
        $buffer .= $indent . '		<li class="divider"></li>';
        $buffer .= "\n";
        $buffer .= $indent . '		<li><a href="#"><i class="icon-off"></i> Logout</a></li>';
        $buffer .= "\n";
        $buffer .= $indent . '	</ul>';
        $buffer .= "\n";
        $buffer .= $indent . '</li>';

        return $buffer;
    }
}
