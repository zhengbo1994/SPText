<?php

class __Mustache_5e4813bdbe5d2e5d2ba3fe14806ca6ce extends Mustache_Template
{
    private $lambdaHelper;

    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $this->lambdaHelper = new Mustache_LambdaHelper($this->mustache, $context);
        $buffer = '';

        $buffer .= $indent . '<li class="grey">';
        $buffer .= "\n";
        $buffer .= $indent . '	<a data-toggle="dropdown" class="dropdown-toggle" href="#">';
        $buffer .= "\n";
        $buffer .= $indent . '		<i class="icon-tasks"></i>';
        $buffer .= "\n";
        $buffer .= $indent . '		<span class="badge badge-grey">';
        $value = $this->resolveValue($context->findDot('layout.topbar_tasks.count'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '</span>';
        $buffer .= "\n";
        $buffer .= $indent . '	</a>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<ul class="pull-right dropdown-navbar dropdown-menu dropdown-caret dropdown-closer">';
        $buffer .= "\n";
        $buffer .= $indent . '		<li class="nav-header">';
        $buffer .= "\n";
        $buffer .= $indent . '			<i class="icon-ok"></i> ';
        $value = $this->resolveValue($context->findDot('layout.topbar_tasks.count'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= ' Tasks to complete';
        $buffer .= "\n";
        $buffer .= $indent . '		</li>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        // 'layout.topbar_tasks.latest' section
        $buffer .= $this->sectionDeccbafbd820ff78dd5c8f3351353308($context, $indent, $context->findDot('layout.topbar_tasks.latest'));
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<li>';
        $buffer .= "\n";
        $buffer .= $indent . '			<a href="#">';
        $buffer .= "\n";
        $buffer .= $indent . '				See tasks with details';
        $buffer .= "\n";
        $buffer .= $indent . '				<i class="icon-arrow-right"></i>';
        $buffer .= "\n";
        $buffer .= $indent . '			</a>';
        $buffer .= "\n";
        $buffer .= $indent . '		</li>';
        $buffer .= "\n";
        $buffer .= $indent . '	</ul>';
        $buffer .= "\n";
        $buffer .= $indent . '</li>';

        return $buffer;
    }

    private function sectionDeccbafbd820ff78dd5c8f3351353308(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
		<li>
			<a href="#">
				<div class="clearfix">
					<span class="pull-left">{{title}}</span>
					<span class="pull-right">{{percentage}}%</span>
				</div>
				<div class="progress progress-mini {{progress-class}}"><div style="width:{{percentage}}%" class="bar"></div></div>
			</a>
		</li>
		';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '		<li>';
                $buffer .= "\n";
                $buffer .= $indent . '			<a href="#">';
                $buffer .= "\n";
                $buffer .= $indent . '				<div class="clearfix">';
                $buffer .= "\n";
                $buffer .= $indent . '					<span class="pull-left">';
                $value = $this->resolveValue($context->find('title'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</span>';
                $buffer .= "\n";
                $buffer .= $indent . '					<span class="pull-right">';
                $value = $this->resolveValue($context->find('percentage'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '%</span>';
                $buffer .= "\n";
                $buffer .= $indent . '				</div>';
                $buffer .= "\n";
                $buffer .= $indent . '				<div class="progress progress-mini ';
                $value = $this->resolveValue($context->find('progress-class'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '"><div style="width:';
                $value = $this->resolveValue($context->find('percentage'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '%" class="bar"></div></div>';
                $buffer .= "\n";
                $buffer .= $indent . '			</a>';
                $buffer .= "\n";
                $buffer .= $indent . '		</li>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }
}
