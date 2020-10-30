<?php

class __Mustache_965316108233064837cf5c950b9f3d96 extends Mustache_Template
{
    private $lambdaHelper;

    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $this->lambdaHelper = new Mustache_LambdaHelper($this->mustache, $context);
        $buffer = '';

        $buffer .= $indent . '<table class="table table-bordered table-striped">';
        $buffer .= "\n";
        $buffer .= $indent . '	<thead>';
        $buffer .= "\n";
        $buffer .= $indent . '		<tr>';
        $buffer .= "\n";
        $buffer .= $indent . '			<th><i class="icon-caret-right blue"></i>name</th>';
        $buffer .= "\n";
        $buffer .= $indent . '			<th><i class="icon-caret-right blue"></i>price</th>';
        $buffer .= "\n";
        $buffer .= $indent . '			<th class="hidden-phone"><i class="icon-caret-right blue"></i>status</th>';
        $buffer .= "\n";
        $buffer .= $indent . '		</tr>';
        $buffer .= "\n";
        $buffer .= $indent . '	</thead>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<tbody>';
        $buffer .= "\n";
        // 'page.domains' section
        $buffer .= $this->sectionE1aa143f221621d86c167444acb6ef76($context, $indent, $context->findDot('page.domains'));
        $buffer .= $indent . '	</tbody>';
        $buffer .= "\n";
        $buffer .= $indent . '</table>';
        $buffer .= "\n";

        return $buffer;
    }

    private function section398929e34b3284539d1f12997d5206e8(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
				 <small><s class="red">{{old_price}}</s></small>
				 <b class="green">{{price}}</b>
				';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '				 <small><s class="red">';
                $value = $this->resolveValue($context->find('old_price'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</s></small>';
                $buffer .= "\n";
                $buffer .= $indent . '				 <b class="green">';
                $value = $this->resolveValue($context->find('price'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</b>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section68aa1dd6b5d82b07a489170692cda565(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
				 <span class="label label-info arrowed-right arrowed-in">on sale</span>
				';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '				 <span class="label label-info arrowed-right arrowed-in">on sale</span>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section3d7f0ee1811e33ab5821a797407b7aa0(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
				 <span class="label label-success arrowed-in arrowed-in-right">approved</span>
				';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '				 <span class="label label-success arrowed-in arrowed-in-right">approved</span>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section284c6775e398800af7277ab004cb0302(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
				 <span class="label label-important arrowed">pending</span>
				';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '				 <span class="label label-important arrowed">pending</span>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section4c9c5cd5ac154e5ed1d5f33fc43fbc2b(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
				 <span class="label arrowed"><s>out of stock</s></span>
				';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '				 <span class="label arrowed"><s>out of stock</s></span>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section39769594668ddd1aab05c401a8ed1700(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
				<span class="label label-warning arrowed arrowed-right">SOLD</span>
				';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '				<span class="label label-warning arrowed arrowed-right">SOLD</span>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionE1aa143f221621d86c167444acb6ef76(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
		<tr>
			<td>{{name}}</td>
			<td>
				{{#old_price}}
				 <small><s class="red">{{old_price}}</s></small>
				 <b class="green">{{price}}</b>
				{{/old_price}}
				{{^old_price}}
				 <b class="blue">{{price}}</b>
				{{/old_price}}
			</td>
			<td class="hidden-phone">
				{{#on_sale}}
				 <span class="label label-info arrowed-right arrowed-in">on sale</span>
				{{/on_sale}}
				{{#approved}}
				 <span class="label label-success arrowed-in arrowed-in-right">approved</span>
				{{/approved}}
				{{#pending}}
				 <span class="label label-important arrowed">pending</span>
				{{/pending}}
				{{#out_of_stock}}
				 <span class="label arrowed"><s>out of stock</s></span>
				{{/out_of_stock}}
				 {{#sold}}
				<span class="label label-warning arrowed arrowed-right">SOLD</span>
				{{/sold}}
			</td>
		</tr>
		';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '		<tr>';
                $buffer .= "\n";
                $buffer .= $indent . '			<td>';
                $value = $this->resolveValue($context->find('name'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</td>';
                $buffer .= "\n";
                $buffer .= $indent . '			<td>';
                $buffer .= "\n";
                // 'old_price' section
                $buffer .= $this->section398929e34b3284539d1f12997d5206e8($context, $indent, $context->find('old_price'));
                // 'old_price' inverted section
                $value = $context->find('old_price');
                if (empty($value)) {
                    
                    $buffer .= $indent . '				 <b class="blue">';
                    $value = $this->resolveValue($context->find('price'), $context, $indent);
                    $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                    $buffer .= '</b>';
                    $buffer .= "\n";
                }
                $buffer .= $indent . '			</td>';
                $buffer .= "\n";
                $buffer .= $indent . '			<td class="hidden-phone">';
                $buffer .= "\n";
                // 'on_sale' section
                $buffer .= $this->section68aa1dd6b5d82b07a489170692cda565($context, $indent, $context->find('on_sale'));
                // 'approved' section
                $buffer .= $this->section3d7f0ee1811e33ab5821a797407b7aa0($context, $indent, $context->find('approved'));
                // 'pending' section
                $buffer .= $this->section284c6775e398800af7277ab004cb0302($context, $indent, $context->find('pending'));
                // 'out_of_stock' section
                $buffer .= $this->section4c9c5cd5ac154e5ed1d5f33fc43fbc2b($context, $indent, $context->find('out_of_stock'));
                // 'sold' section
                $buffer .= $this->section39769594668ddd1aab05c401a8ed1700($context, $indent, $context->find('sold'));
                $buffer .= $indent . '			</td>';
                $buffer .= "\n";
                $buffer .= $indent . '		</tr>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }
}
