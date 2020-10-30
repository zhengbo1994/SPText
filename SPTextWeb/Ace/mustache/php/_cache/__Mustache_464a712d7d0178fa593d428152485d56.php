<?php

class __Mustache_464a712d7d0178fa593d428152485d56 extends Mustache_Template
{
    private $lambdaHelper;

    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $this->lambdaHelper = new Mustache_LambdaHelper($this->mustache, $context);
        $buffer = '';

        $buffer .= $indent . '<li';
        // 'class' section
        $buffer .= $this->section0a5f15f888b57871f529847cdcbb8074($context, $indent, $context->find('class'));
        $buffer .= '>';
        $buffer .= '';
        $buffer .= "\n";
        $buffer .= $indent . '  <a href="';
        // 'link' section
        $buffer .= $this->section4549e92ee88b796fb2bda9263e955a73($context, $indent, $context->find('link'));
        // 'link' inverted section
        $value = $context->find('link');
        if (empty($value)) {
            
            $buffer .= '#';
        }
        $buffer .= '"';
        // 'submenu?' section
        $buffer .= $this->section2fd0628d963daf86d2fde973edd59498($context, $indent, $context->find('submenu?'));
        $buffer .= '>';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        // 'icon' section
        $buffer .= $this->sectionF1b76670df495f33609951d1f9e98327($context, $indent, $context->find('icon'));
        $buffer .= '';
        $buffer .= "\n";
        // 'level-1' section
        $buffer .= $this->section29c20280b586c364710c8dd15d94e361($context, $indent, $context->find('level-1'));
        // 'level-2' section
        $buffer .= $this->section76d69aba09541a7a94536b64cf3be481($context, $indent, $context->find('level-2'));
        $buffer .= $indent . '	';
        // 'level-1' section
        $buffer .= $this->section4edd5c39a39d16cc57a5f871d3560d3e($context, $indent, $context->find('level-1'));
        $buffer .= '';
        $buffer .= "\n";
        $buffer .= $indent . '  </a>';
        $buffer .= "\n";
        $buffer .= $indent . '  ';
        $buffer .= "\n";
        // 'submenu?' section
        $buffer .= $this->sectionDde2145ca6771e8dab36bd136932a8a5($context, $indent, $context->find('submenu?'));
        $buffer .= $indent . '</li>';

        return $buffer;
    }

    private function section0a5f15f888b57871f529847cdcbb8074(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = ' class="{{class}}"';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= ' class="';
                $value = $this->resolveValue($context->find('class'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '"';
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionF4e79cc86c3196e5bf3c56812364fd5d(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '{{link}}';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $value = $this->resolveValue($context->find('link'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section4549e92ee88b796fb2bda9263e955a73(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '{{#createLink}}{{link}}{{/createLink}}';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                // 'createLink' section
                $buffer .= $this->sectionF4e79cc86c3196e5bf3c56812364fd5d($context, $indent, $context->find('createLink'));
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section2fd0628d963daf86d2fde973edd59498(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = ' class="dropdown-toggle"';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= ' class="dropdown-toggle"';
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionF1b76670df495f33609951d1f9e98327(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '<i class="{{icon}}"></i>';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= '<i class="';
                $value = $this->resolveValue($context->find('icon'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '"></i>';
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section29c20280b586c364710c8dd15d94e361(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
	 <span>{{title}}</span>
	';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '	 <span>';
                $value = $this->resolveValue($context->find('title'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</span>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section76d69aba09541a7a94536b64cf3be481(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
	 <i class="icon-double-angle-right"></i>
	 {{title}}
	';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '	 <i class="icon-double-angle-right"></i>';
                $buffer .= "\n";
                $buffer .= $indent . '	 ';
                $value = $this->resolveValue($context->find('title'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionAd1a46197596c8dc73bfadaedf9bcf40(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '<b class="arrow icon-angle-down"></b>';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= '<b class="arrow icon-angle-down"></b>';
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section4edd5c39a39d16cc57a5f871d3560d3e(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '{{#submenu?}}<b class="arrow icon-angle-down"></b>{{/submenu?}}';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                // 'submenu?' section
                $buffer .= $this->sectionAd1a46197596c8dc73bfadaedf9bcf40($context, $indent, $context->find('submenu?'));
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionF03067bf6a6612fbe2d592db24dffd26(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
		{{> layout.sidenav.items}}
	';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                if ($partial = $this->mustache->loadPartial('layout.sidenav.items')) {
                    $buffer .= $partial->renderInternal($context, '		');
                }
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionDde2145ca6771e8dab36bd136932a8a5(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '{{! if we have submenu items, print them recursively }}
	<ul class="submenu">
	{{#submenu}}
		{{> layout.sidenav.items}}
	{{/submenu}}
	</ul>
  ';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '	<ul class="submenu">';
                $buffer .= "\n";
                // 'submenu' section
                $buffer .= $this->sectionF03067bf6a6612fbe2d592db24dffd26($context, $indent, $context->find('submenu'));
                $buffer .= $indent . '	</ul>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }
}
