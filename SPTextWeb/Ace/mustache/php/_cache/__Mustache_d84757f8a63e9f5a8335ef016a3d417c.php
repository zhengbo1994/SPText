<?php

class __Mustache_d84757f8a63e9f5a8335ef016a3d417c extends Mustache_Template
{
    private $lambdaHelper;

    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $this->lambdaHelper = new Mustache_LambdaHelper($this->mustache, $context);
        $buffer = '';

        $buffer .= $indent . '<ul id="tasks" class="item-list">';
        $buffer .= "\n";
        // 'page.tasks' section
        $buffer .= $this->sectionAfc848b494557190a3d014a3a03c99b7($context, $indent, $context->findDot('page.tasks'));
        $buffer .= $indent . '</ul>';

        return $buffer;
    }

    private function section91f123b8467f1d28f105e647e0d3bb74(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
		<div class="pull-right easy-pie-chart percentage" data-size="30" data-color="#ECCB71" data-percent="{{progress}}">
			<span class="percent">{{progress}}</span>%
		</div>
		';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '		<div class="pull-right easy-pie-chart percentage" data-size="30" data-color="#ECCB71" data-percent="';
                $value = $this->resolveValue($context->find('progress'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '">';
                $buffer .= "\n";
                $buffer .= $indent . '			<span class="percent">';
                $value = $this->resolveValue($context->find('progress'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</span>%';
                $buffer .= "\n";
                $buffer .= $indent . '		</div>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionB519119ec806f3a688d874e198f94f06(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
		<div class="pull-right">
			<div class="btn-group">
				<button class="btn btn-mini btn-info"><i class="icon-edit bigger-125"></i></button>
				<button class="btn btn-mini btn-danger "><i class="icon-trash bigger-125"></i></button>
				<button class="btn btn-mini btn-yellow"><i class="icon-flag bigger-125"></i></button>
			</div>
		</div>
		';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '		<div class="pull-right">';
                $buffer .= "\n";
                $buffer .= $indent . '			<div class="btn-group">';
                $buffer .= "\n";
                $buffer .= $indent . '				<button class="btn btn-mini btn-info"><i class="icon-edit bigger-125"></i></button>';
                $buffer .= "\n";
                $buffer .= $indent . '				<button class="btn btn-mini btn-danger "><i class="icon-trash bigger-125"></i></button>';
                $buffer .= "\n";
                $buffer .= $indent . '				<button class="btn btn-mini btn-yellow"><i class="icon-flag bigger-125"></i></button>';
                $buffer .= "\n";
                $buffer .= $indent . '			</div>';
                $buffer .= "\n";
                $buffer .= $indent . '		</div>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section6306852ea6a689ae7c6a49f8b47dff05(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
		<div class="inline pull-right position-relative">
			<button class="btn btn-minier bigger btn-yellow dropdown-toggle" data-toggle="dropdown"><i class="icon-angle-down icon-only bigger-120"></i></button>
			<ul class="dropdown-menu dropdown-icon-only dropdown-yellow pull-right dropdown-caret dropdown-closer">
				<li><a href="#" class="tooltip-success" data-rel="tooltip" title="Mark&nbsp;as&nbsp;done" data-placement="left"><span class="green"><i class="icon-ok"></i></span></a></li>
				<li><a href="#" class="tooltip-error" data-rel="tooltip" title="Delete" data-placement="left"><span class="red"><i class="icon-trash"></i></span></a></li>
			</ul>
		</div>
		';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '		<div class="inline pull-right position-relative">';
                $buffer .= "\n";
                $buffer .= $indent . '			<button class="btn btn-minier bigger btn-yellow dropdown-toggle" data-toggle="dropdown"><i class="icon-angle-down icon-only bigger-120"></i></button>';
                $buffer .= "\n";
                $buffer .= $indent . '			<ul class="dropdown-menu dropdown-icon-only dropdown-yellow pull-right dropdown-caret dropdown-closer">';
                $buffer .= "\n";
                $buffer .= $indent . '				<li><a href="#" class="tooltip-success" data-rel="tooltip" title="Mark&nbsp;as&nbsp;done" data-placement="left"><span class="green"><i class="icon-ok"></i></span></a></li>';
                $buffer .= "\n";
                $buffer .= $indent . '				<li><a href="#" class="tooltip-error" data-rel="tooltip" title="Delete" data-placement="left"><span class="red"><i class="icon-trash"></i></span></a></li>';
                $buffer .= "\n";
                $buffer .= $indent . '			</ul>';
                $buffer .= "\n";
                $buffer .= $indent . '		</div>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionAfc848b494557190a3d014a3a03c99b7(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
	<li class="item-{{color}}">
		<label class="inline"><input type="checkbox" /><span class="lbl"> {{title}}</span></label>
		{{#progress}}
		<div class="pull-right easy-pie-chart percentage" data-size="30" data-color="#ECCB71" data-percent="{{progress}}">
			<span class="percent">{{progress}}</span>%
		</div>
		{{/progress}}
		{{#toolbar}}
		<div class="pull-right">
			<div class="btn-group">
				<button class="btn btn-mini btn-info"><i class="icon-edit bigger-125"></i></button>
				<button class="btn btn-mini btn-danger "><i class="icon-trash bigger-125"></i></button>
				<button class="btn btn-mini btn-yellow"><i class="icon-flag bigger-125"></i></button>
			</div>
		</div>
		{{/toolbar}}
		{{#menu}}
		<div class="inline pull-right position-relative">
			<button class="btn btn-minier bigger btn-yellow dropdown-toggle" data-toggle="dropdown"><i class="icon-angle-down icon-only bigger-120"></i></button>
			<ul class="dropdown-menu dropdown-icon-only dropdown-yellow pull-right dropdown-caret dropdown-closer">
				<li><a href="#" class="tooltip-success" data-rel="tooltip" title="Mark&nbsp;as&nbsp;done" data-placement="left"><span class="green"><i class="icon-ok"></i></span></a></li>
				<li><a href="#" class="tooltip-error" data-rel="tooltip" title="Delete" data-placement="left"><span class="red"><i class="icon-trash"></i></span></a></li>
			</ul>
		</div>
		{{/menu}}
	</li>
	';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '	<li class="item-';
                $value = $this->resolveValue($context->find('color'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '">';
                $buffer .= "\n";
                $buffer .= $indent . '		<label class="inline"><input type="checkbox" /><span class="lbl"> ';
                $value = $this->resolveValue($context->find('title'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</span></label>';
                $buffer .= "\n";
                // 'progress' section
                $buffer .= $this->section91f123b8467f1d28f105e647e0d3bb74($context, $indent, $context->find('progress'));
                // 'toolbar' section
                $buffer .= $this->sectionB519119ec806f3a688d874e198f94f06($context, $indent, $context->find('toolbar'));
                // 'menu' section
                $buffer .= $this->section6306852ea6a689ae7c6a49f8b47dff05($context, $indent, $context->find('menu'));
                $buffer .= $indent . '	</li>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }
}
