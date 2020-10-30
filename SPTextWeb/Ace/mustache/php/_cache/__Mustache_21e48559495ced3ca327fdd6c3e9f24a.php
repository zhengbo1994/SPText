<?php

class __Mustache_21e48559495ced3ca327fdd6c3e9f24a extends Mustache_Template
{
    private $lambdaHelper;

    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $this->lambdaHelper = new Mustache_LambdaHelper($this->mustache, $context);
        $buffer = '';

        $buffer .= $indent . '<div class="span2 pricing-span">';
        $buffer .= "\n";
        $buffer .= $indent . '	<div class="widget-box pricing-box-small">';
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
        $buffer .= $indent . '		 <div class="widget-main no-padding">';
        $buffer .= "\n";
        $buffer .= $indent . '			<ul class="unstyled list-striped pricing-table">';
        $buffer .= "\n";
        // 'values' section
        $buffer .= $this->sectionB49c6256f0dc9320f6d8aa1a5f4dbcdd($context, $indent, $context->find('values'));
        $buffer .= $indent . '			</ul>';
        $buffer .= "\n";
        $buffer .= $indent . '			';
        $buffer .= "\n";
        $buffer .= $indent . '			<div class="price">';
        $buffer .= "\n";
        $buffer .= $indent . '				<span class="label label-large label-inverse arrowed-in arrowed-in-right">';
        $buffer .= "\n";
        $buffer .= $indent . '					';
        $value = $this->resolveValue($context->find('price'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '<small>/';
        $value = $this->resolveValue($context->find('duration'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '</small>';
        $buffer .= "\n";
        $buffer .= $indent . '				</span>';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '		 <div>';
        $buffer .= "\n";
        $buffer .= $indent . '			<a href="#" class="btn btn-block btn-small btn-';
        $value = $this->resolveValue($context->find('btn-color'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '"><span>Buy</span></a>';
        $buffer .= "\n";
        $buffer .= $indent . '		 </div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div>';
        $buffer .= "\n";
        $buffer .= $indent . '	</div>';
        $buffer .= "\n";
        $buffer .= $indent . '</div>';

        return $buffer;
    }

    private function section8c5bacff6564d34f48a9f56cc762d099(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
						<i class="icon-ok green"></i>
						';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '						<i class="icon-ok green"></i>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionCac270ff1183298771df7ab7e1d07351(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
						<i class="icon-remove red"></i>
						';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '						<i class="icon-remove red"></i>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionB49c6256f0dc9320f6d8aa1a5f4dbcdd(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
					<li>
						{{#available}}
						<i class="icon-ok green"></i>
						{{/available}}
						{{#unavailable}}
						<i class="icon-remove red"></i>
						{{/unavailable}}						
						{{value}}
					</li>
				';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '					<li>';
                $buffer .= "\n";
                // 'available' section
                $buffer .= $this->section8c5bacff6564d34f48a9f56cc762d099($context, $indent, $context->find('available'));
                // 'unavailable' section
                $buffer .= $this->sectionCac270ff1183298771df7ab7e1d07351($context, $indent, $context->find('unavailable'));
                $buffer .= $indent . '						';
                $value = $this->resolveValue($context->find('value'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= "\n";
                $buffer .= $indent . '					</li>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }
}
