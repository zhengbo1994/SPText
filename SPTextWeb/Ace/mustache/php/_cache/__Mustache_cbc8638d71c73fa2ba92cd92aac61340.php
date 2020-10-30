<?php

class __Mustache_cbc8638d71c73fa2ba92cd92aac61340 extends Mustache_Template
{
    private $lambdaHelper;

    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $this->lambdaHelper = new Mustache_LambdaHelper($this->mustache, $context);
        $buffer = '';

        $buffer .= $indent . '<div class="row-fluid">';
        $buffer .= "\n";
        // 'page.packages-large' section
        $buffer .= $this->section4e3910421f2f909689030fd3f08d8f8e($context, $indent, $context->findDot('page.packages-large'));
        $buffer .= $indent . '</div>';
        $buffer .= "\n";
        $buffer .= "\n";
        $buffer .= "\n";
        $buffer .= $indent . '<div class="space-24"></div>';
        $buffer .= "\n";
        $buffer .= "\n";
        $buffer .= "\n";
        $buffer .= $indent . '<h3 class="header smaller red">Small Style</h3>';
        $buffer .= "\n";
        $buffer .= $indent . '<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="span3">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="widget-box transparent">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-header">';
        $buffer .= "\n";
        $buffer .= $indent . '				<h5 class="bigger lighter">Package Name</h5>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '			 <div class="widget-main no-padding">';
        $buffer .= "\n";
        $buffer .= $indent . '				<ul class="unstyled list-striped pricing-table-header">';
        $buffer .= "\n";
        $buffer .= $indent . '					<li>Disk Space </li>';
        $buffer .= "\n";
        $buffer .= $indent . '					<li>Bandwidth </li>';
        $buffer .= "\n";
        $buffer .= $indent . '					<li>Email Accounts </li>';
        $buffer .= "\n";
        $buffer .= $indent . '					<li>MySQL Databases </li>';
        $buffer .= "\n";
        $buffer .= $indent . '					<li>Ad Credit </li>';
        $buffer .= "\n";
        $buffer .= $indent . '					<li>Free Domain </li>';
        $buffer .= "\n";
        $buffer .= $indent . '				</ul>';
        $buffer .= "\n";
        $buffer .= $indent . '			 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= "\n";
        // 'page.packages-small' section
        $buffer .= $this->section126024565e891dc088f76f40772604bf($context, $indent, $context->findDot('page.packages-small'));
        $buffer .= $indent . '</div>';

        return $buffer;
    }

    private function section4e3910421f2f909689030fd3f08d8f8e(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
	{{> page.package-large}}
	';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                if ($partial = $this->mustache->loadPartial('page.package-large')) {
                    $buffer .= $partial->renderInternal($context, '	');
                }
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section126024565e891dc088f76f40772604bf(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
	{{> page.package-small}}
	';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                if ($partial = $this->mustache->loadPartial('page.package-small')) {
                    $buffer .= $partial->renderInternal($context, '	');
                }
                $context->pop();
            }
        }
    
        return $buffer;
    }
}
