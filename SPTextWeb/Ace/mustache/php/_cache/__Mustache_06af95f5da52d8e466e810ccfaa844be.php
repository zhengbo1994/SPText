<?php

class __Mustache_06af95f5da52d8e466e810ccfaa844be extends Mustache_Template
{
    private $lambdaHelper;

    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $this->lambdaHelper = new Mustache_LambdaHelper($this->mustache, $context);
        $buffer = '';

        $buffer .= $indent . '<div class="span3">';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="widget-box pricing-box">';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="widget-header header-color-';
        $value = $this->resolveValue($context->find('title-color'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '">';
        $buffer .= "\n";
        $buffer .= $indent . '			<h5 class="bigger lighter">';
        $value = $this->resolveValue($context->find('title'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '</h5>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="widget-body">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="widget-main">';
        $buffer .= "\n";
        $buffer .= $indent . '				<ul class="unstyled spaced2">';
        $buffer .= "\n";
        // 'features' section
        $buffer .= $this->section543fffe6326adb29a43ae19602de9ac9($context, $indent, $context->find('features'));
        $buffer .= $indent . '				</ul>';
        $buffer .= "\n";
        $buffer .= $indent . '				';
        $buffer .= "\n";
        $buffer .= $indent . '				<hr />';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="price">';
        $buffer .= "\n";
        $buffer .= $indent . '					';
        $value = $this->resolveValue($context->find('price'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '<small>/';
        $value = $this->resolveValue($context->find('duration'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '</small>';
        $buffer .= "\n";
        $buffer .= $indent . '				</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '			';
        $buffer .= "\n";
        $buffer .= $indent . '			<div>';
        $buffer .= "\n";
        $buffer .= $indent . '				<a href="#" class="btn btn-block btn-';
        $value = $this->resolveValue($context->find('btn-color'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '"><span>Buy</span></a>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '</div>';

        return $buffer;
    }

    private function section397b2c8efbb931aac98937ab49fceaea(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
						<li><i class="icon-remove red"></i> {{name}} </li>
						';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '						<li><i class="icon-remove red"></i> ';
                $value = $this->resolveValue($context->find('name'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= ' </li>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section543fffe6326adb29a43ae19602de9ac9(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
						{{^unavailable}}
						<li><i class="icon-ok green"></i> {{name}} </li>
						{{/unavailable}}
						{{#unavailable}}
						<li><i class="icon-remove red"></i> {{name}} </li>
						{{/unavailable}}
					';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                // 'unavailable' inverted section
                $value = $context->find('unavailable');
                if (empty($value)) {
                    
                    $buffer .= $indent . '						<li><i class="icon-ok green"></i> ';
                    $value = $this->resolveValue($context->find('name'), $context, $indent);
                    $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                    $buffer .= ' </li>';
                    $buffer .= "\n";
                }
                // 'unavailable' section
                $buffer .= $this->section397b2c8efbb931aac98937ab49fceaea($context, $indent, $context->find('unavailable'));
                $context->pop();
            }
        }
    
        return $buffer;
    }
}
