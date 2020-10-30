<?php

class __Mustache_c7aff812777f0f03fb793cb24c4c2744 extends Mustache_Template
{
    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $buffer = '';

        $buffer .= $indent . '<div id="sidebar-shortcuts">';
        $buffer .= "\n";
        $buffer .= $indent . '	<div id="sidebar-shortcuts-large">';
        $buffer .= "\n";
        $buffer .= $indent . '		<button class="btn btn-small btn-success"><i class="icon-signal"></i></button>';
        $buffer .= "\n";
        $buffer .= $indent . '		<button class="btn btn-small btn-info"><i class="icon-pencil"></i></button>';
        $buffer .= "\n";
        $buffer .= $indent . '		<button class="btn btn-small btn-warning"><i class="icon-group"></i></button>';
        $buffer .= "\n";
        $buffer .= $indent . '		<button class="btn btn-small btn-danger"><i class="icon-cogs"></i></button>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	<div id="sidebar-shortcuts-mini">';
        $buffer .= "\n";
        $buffer .= $indent . '		<span class="btn btn-success"></span>';
        $buffer .= "\n";
        $buffer .= $indent . '		<span class="btn btn-info"></span>';
        $buffer .= "\n";
        $buffer .= $indent . '		<span class="btn btn-warning"></span>';
        $buffer .= "\n";
        $buffer .= $indent . '		<span class="btn btn-danger"></span>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '</div><!-- #sidebar-shortcuts -->';

        return $buffer;
    }
}
