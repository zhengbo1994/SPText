<?php

class __Mustache_c450ea14f7b772799a7777db83ed4c85 extends Mustache_Template
{
    private $lambdaHelper;

    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $this->lambdaHelper = new Mustache_LambdaHelper($this->mustache, $context);
        $buffer = '';

        $buffer .= $indent . '<div class="space-6"></div>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        // 'page.invoice' section
        $buffer .= $this->section09a93690005efc14a6c9dae30f81839a($context, $indent, $context->findDot('page.invoice'));

        return $buffer;
    }

    private function sectionC137818103e1f222a380fed523bdf4d9(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
								<li><i class="icon-caret-right green"></i>{{.}}</li>
								';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '								<li><i class="icon-caret-right green"></i>';
                $value = $this->resolveValue($context->last(), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</li>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section614ed902c2e32d58b34c993452240efb(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
							<ul class="unstyled spaced">
								{{#address}}
								<li><i class="icon-caret-right green"></i>{{.}}</li>
								{{/address}}
								<li class="divider"></li>
								<li><i class="icon-caret-right green"></i>
									{{contact}}
								</li>
							</ul>
							';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '							<ul class="unstyled spaced">';
                $buffer .= "\n";
                // 'address' section
                $buffer .= $this->sectionC137818103e1f222a380fed523bdf4d9($context, $indent, $context->find('address'));
                $buffer .= $indent . '								<li class="divider"></li>';
                $buffer .= "\n";
                $buffer .= $indent . '								<li><i class="icon-caret-right green"></i>';
                $buffer .= "\n";
                $buffer .= $indent . '									';
                $value = $this->resolveValue($context->find('contact'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '';
                $buffer .= "\n";
                $buffer .= $indent . '								</li>';
                $buffer .= "\n";
                $buffer .= $indent . '							</ul>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionFfe77a6e617fa0ed95652e92d4e81a26(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = ' {{discount}} ';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= ' ';
                $value = $this->resolveValue($context->find('discount'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= ' ';
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section8c0d1a01b50770d0343890942c08a743(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
							<tr>
								<td class="center">{{id}}</td>
								<td>{{{name}}}</td>
								<td class="hidden-phone">
									{{description}}
								</td>
								<td class="hidden-480">
								{{#discount}} {{discount}} {{/discount}}
								{{^discount}} --- {{/discount}}
								</td>
								<td>{{total}}</td>
							</tr>
						';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '							<tr>';
                $buffer .= "\n";
                $buffer .= $indent . '								<td class="center">';
                $value = $this->resolveValue($context->find('id'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</td>';
                $buffer .= "\n";
                $buffer .= $indent . '								<td>';
                $value = $this->resolveValue($context->find('name'), $context, $indent);
                $buffer .= $value;
                $buffer .= '</td>';
                $buffer .= "\n";
                $buffer .= $indent . '								<td class="hidden-phone">';
                $buffer .= "\n";
                $buffer .= $indent . '									';
                $value = $this->resolveValue($context->find('description'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '';
                $buffer .= "\n";
                $buffer .= $indent . '								</td>';
                $buffer .= "\n";
                $buffer .= $indent . '								<td class="hidden-480">';
                $buffer .= "\n";
                $buffer .= $indent . '								';
                // 'discount' section
                $buffer .= $this->sectionFfe77a6e617fa0ed95652e92d4e81a26($context, $indent, $context->find('discount'));
                $buffer .= '';
                $buffer .= "\n";
                $buffer .= $indent . '								';
                // 'discount' inverted section
                $value = $context->find('discount');
                if (empty($value)) {
                    
                    $buffer .= ' --- ';
                }
                $buffer .= '';
                $buffer .= "\n";
                $buffer .= $indent . '								</td>';
                $buffer .= "\n";
                $buffer .= $indent . '								<td>';
                $value = $this->resolveValue($context->find('total'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</td>';
                $buffer .= "\n";
                $buffer .= $indent . '							</tr>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section09a93690005efc14a6c9dae30f81839a(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
<div class="row-fluid">
	<div class="span10 offset1">
	 <div class="widget-box transparent invoice-box">
	  <div class="widget-header widget-header-large">
		<h3 class="grey lighter pull-left position-relative">
			<i class="icon-leaf green"></i>
			{{title}}
		</h3>
				
		<div class="widget-toolbar no-border invoice-info">
			<span class="invoice-info-label">Invoice:</span> <span class="red">#{{number}}</span>
			<br />
			<span class="invoice-info-label">Date:</span> <span class="blue">{{date}}</span>
		</div>
		
		<div class="widget-toolbar hidden-480">
			<a href="#"><i class="icon-print"></i></a>
		</div>
	  </div>
	  
	  
	  <div class="widget-body"><div class="widget-main padding-24">
			<div class="row-fluid">
				<div class="row-fluid">
					<div class="span6">
						<div class="row-fluid">
							<div class="span12 label label-large label-info arrowed-in arrowed-right"><b>Company Info</b></div>
						</div>
						<div class="row-fluid">
							<ul class="unstyled spaced">
								<li><i class="icon-caret-right blue"></i>Street, City</li>
								<li><i class="icon-caret-right blue"></i>Zip Code</li>
								<li><i class="icon-caret-right blue"></i>State, Country</li>
								<li><i class="icon-caret-right blue"></i>Phone: <b class="red">111-111-111</b></li>
								<li class="divider"></li>
								<li><i class="icon-caret-right blue"></i>
									Paymant Info
								</li>
							</ul>
						</div>
					</div><!--/span-->
					
					<div class="span6">
						<div class="row-fluid">
							<div class="span12 label label-large label-success arrowed-in arrowed-right"><b>Customer Info</b></div>
						</div>
						<div class="row-fluid">
							{{#customer}}
							<ul class="unstyled spaced">
								{{#address}}
								<li><i class="icon-caret-right green"></i>{{.}}</li>
								{{/address}}
								<li class="divider"></li>
								<li><i class="icon-caret-right green"></i>
									{{contact}}
								</li>
							</ul>
							{{/customer}}
						</div>
					</div><!--/span-->
				</div><!--row-->
				
				<div class="space"></div>
				
				<div class="row-fluid">

					<table class="table table-striped table-bordered">
						<thead>
							<tr>
								<th class="center">#</th>
								<th>Product</th>
								<th class="hidden-phone">Description</th>
								<th class="hidden-480">Discount</th>
								<th>Total</th>
							</tr>
						</thead>
						
						<tbody>
						{{#products}}
							<tr>
								<td class="center">{{id}}</td>
								<td>{{{name}}}</td>
								<td class="hidden-phone">
									{{description}}
								</td>
								<td class="hidden-480">
								{{#discount}} {{discount}} {{/discount}}
								{{^discount}} --- {{/discount}}
								</td>
								<td>{{total}}</td>
							</tr>
						{{/products}}
						</tbody>
					</table>
					
				</div>
				
				<div class="hr hr8 hr-double hr-dotted"></div>
				
				<div class="row-fluid">
					<div class="span5 pull-right">
						<h4 class="pull-right">
						Total amount : <span class="red">{{total}}</span>
						</h4>
					</div>
					
					<div class="span7 pull-left">
						Extra Information
					</div>
				</div>
				
				<div class="space-6"></div>
				
				<div class="row-fluid">
					<div class="span12 well">
						Thank you for choosing Ace Company products.
						We believe you will be satisfied by our services.
					</div>
				</div>
				
			</div>

		</div></div>
	 </div>
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
                $buffer .= $indent . '<div class="row-fluid">';
                $buffer .= "\n";
                $buffer .= $indent . '	<div class="span10 offset1">';
                $buffer .= "\n";
                $buffer .= $indent . '	 <div class="widget-box transparent invoice-box">';
                $buffer .= "\n";
                $buffer .= $indent . '	  <div class="widget-header widget-header-large">';
                $buffer .= "\n";
                $buffer .= $indent . '		<h3 class="grey lighter pull-left position-relative">';
                $buffer .= "\n";
                $buffer .= $indent . '			<i class="icon-leaf green"></i>';
                $buffer .= "\n";
                $buffer .= $indent . '			';
                $value = $this->resolveValue($context->find('title'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '';
                $buffer .= "\n";
                $buffer .= $indent . '		</h3>';
                $buffer .= "\n";
                $buffer .= $indent . '				';
                $buffer .= "\n";
                $buffer .= $indent . '		<div class="widget-toolbar no-border invoice-info">';
                $buffer .= "\n";
                $buffer .= $indent . '			<span class="invoice-info-label">Invoice:</span> <span class="red">#';
                $value = $this->resolveValue($context->find('number'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</span>';
                $buffer .= "\n";
                $buffer .= $indent . '			<br />';
                $buffer .= "\n";
                $buffer .= $indent . '			<span class="invoice-info-label">Date:</span> <span class="blue">';
                $value = $this->resolveValue($context->find('date'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</span>';
                $buffer .= "\n";
                $buffer .= $indent . '		</div>';
                $buffer .= "\n";
                $buffer .= $indent . '		';
                $buffer .= "\n";
                $buffer .= $indent . '		<div class="widget-toolbar hidden-480">';
                $buffer .= "\n";
                $buffer .= $indent . '			<a href="#"><i class="icon-print"></i></a>';
                $buffer .= "\n";
                $buffer .= $indent . '		</div>';
                $buffer .= "\n";
                $buffer .= $indent . '	  </div>';
                $buffer .= "\n";
                $buffer .= $indent . '	  ';
                $buffer .= "\n";
                $buffer .= $indent . '	  ';
                $buffer .= "\n";
                $buffer .= $indent . '	  <div class="widget-body"><div class="widget-main padding-24">';
                $buffer .= "\n";
                $buffer .= $indent . '			<div class="row-fluid">';
                $buffer .= "\n";
                $buffer .= $indent . '				<div class="row-fluid">';
                $buffer .= "\n";
                $buffer .= $indent . '					<div class="span6">';
                $buffer .= "\n";
                $buffer .= $indent . '						<div class="row-fluid">';
                $buffer .= "\n";
                $buffer .= $indent . '							<div class="span12 label label-large label-info arrowed-in arrowed-right"><b>Company Info</b></div>';
                $buffer .= "\n";
                $buffer .= $indent . '						</div>';
                $buffer .= "\n";
                $buffer .= $indent . '						<div class="row-fluid">';
                $buffer .= "\n";
                $buffer .= $indent . '							<ul class="unstyled spaced">';
                $buffer .= "\n";
                $buffer .= $indent . '								<li><i class="icon-caret-right blue"></i>Street, City</li>';
                $buffer .= "\n";
                $buffer .= $indent . '								<li><i class="icon-caret-right blue"></i>Zip Code</li>';
                $buffer .= "\n";
                $buffer .= $indent . '								<li><i class="icon-caret-right blue"></i>State, Country</li>';
                $buffer .= "\n";
                $buffer .= $indent . '								<li><i class="icon-caret-right blue"></i>Phone: <b class="red">111-111-111</b></li>';
                $buffer .= "\n";
                $buffer .= $indent . '								<li class="divider"></li>';
                $buffer .= "\n";
                $buffer .= $indent . '								<li><i class="icon-caret-right blue"></i>';
                $buffer .= "\n";
                $buffer .= $indent . '									Paymant Info';
                $buffer .= "\n";
                $buffer .= $indent . '								</li>';
                $buffer .= "\n";
                $buffer .= $indent . '							</ul>';
                $buffer .= "\n";
                $buffer .= $indent . '						</div>';
                $buffer .= "\n";
                $buffer .= $indent . '					</div><!--/span-->';
                $buffer .= "\n";
                $buffer .= $indent . '					';
                $buffer .= "\n";
                $buffer .= $indent . '					<div class="span6">';
                $buffer .= "\n";
                $buffer .= $indent . '						<div class="row-fluid">';
                $buffer .= "\n";
                $buffer .= $indent . '							<div class="span12 label label-large label-success arrowed-in arrowed-right"><b>Customer Info</b></div>';
                $buffer .= "\n";
                $buffer .= $indent . '						</div>';
                $buffer .= "\n";
                $buffer .= $indent . '						<div class="row-fluid">';
                $buffer .= "\n";
                // 'customer' section
                $buffer .= $this->section614ed902c2e32d58b34c993452240efb($context, $indent, $context->find('customer'));
                $buffer .= $indent . '						</div>';
                $buffer .= "\n";
                $buffer .= $indent . '					</div><!--/span-->';
                $buffer .= "\n";
                $buffer .= $indent . '				</div><!--row-->';
                $buffer .= "\n";
                $buffer .= $indent . '				';
                $buffer .= "\n";
                $buffer .= $indent . '				<div class="space"></div>';
                $buffer .= "\n";
                $buffer .= $indent . '				';
                $buffer .= "\n";
                $buffer .= $indent . '				<div class="row-fluid">';
                $buffer .= "\n";
                $buffer .= $indent . '';
                $buffer .= "\n";
                $buffer .= $indent . '					<table class="table table-striped table-bordered">';
                $buffer .= "\n";
                $buffer .= $indent . '						<thead>';
                $buffer .= "\n";
                $buffer .= $indent . '							<tr>';
                $buffer .= "\n";
                $buffer .= $indent . '								<th class="center">#</th>';
                $buffer .= "\n";
                $buffer .= $indent . '								<th>Product</th>';
                $buffer .= "\n";
                $buffer .= $indent . '								<th class="hidden-phone">Description</th>';
                $buffer .= "\n";
                $buffer .= $indent . '								<th class="hidden-480">Discount</th>';
                $buffer .= "\n";
                $buffer .= $indent . '								<th>Total</th>';
                $buffer .= "\n";
                $buffer .= $indent . '							</tr>';
                $buffer .= "\n";
                $buffer .= $indent . '						</thead>';
                $buffer .= "\n";
                $buffer .= $indent . '						';
                $buffer .= "\n";
                $buffer .= $indent . '						<tbody>';
                $buffer .= "\n";
                // 'products' section
                $buffer .= $this->section8c0d1a01b50770d0343890942c08a743($context, $indent, $context->find('products'));
                $buffer .= $indent . '						</tbody>';
                $buffer .= "\n";
                $buffer .= $indent . '					</table>';
                $buffer .= "\n";
                $buffer .= $indent . '					';
                $buffer .= "\n";
                $buffer .= $indent . '				</div>';
                $buffer .= "\n";
                $buffer .= $indent . '				';
                $buffer .= "\n";
                $buffer .= $indent . '				<div class="hr hr8 hr-double hr-dotted"></div>';
                $buffer .= "\n";
                $buffer .= $indent . '				';
                $buffer .= "\n";
                $buffer .= $indent . '				<div class="row-fluid">';
                $buffer .= "\n";
                $buffer .= $indent . '					<div class="span5 pull-right">';
                $buffer .= "\n";
                $buffer .= $indent . '						<h4 class="pull-right">';
                $buffer .= "\n";
                $buffer .= $indent . '						Total amount : <span class="red">';
                $value = $this->resolveValue($context->find('total'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</span>';
                $buffer .= "\n";
                $buffer .= $indent . '						</h4>';
                $buffer .= "\n";
                $buffer .= $indent . '					</div>';
                $buffer .= "\n";
                $buffer .= $indent . '					';
                $buffer .= "\n";
                $buffer .= $indent . '					<div class="span7 pull-left">';
                $buffer .= "\n";
                $buffer .= $indent . '						Extra Information';
                $buffer .= "\n";
                $buffer .= $indent . '					</div>';
                $buffer .= "\n";
                $buffer .= $indent . '				</div>';
                $buffer .= "\n";
                $buffer .= $indent . '				';
                $buffer .= "\n";
                $buffer .= $indent . '				<div class="space-6"></div>';
                $buffer .= "\n";
                $buffer .= $indent . '				';
                $buffer .= "\n";
                $buffer .= $indent . '				<div class="row-fluid">';
                $buffer .= "\n";
                $buffer .= $indent . '					<div class="span12 well">';
                $buffer .= "\n";
                $buffer .= $indent . '						Thank you for choosing Ace Company products.';
                $buffer .= "\n";
                $buffer .= $indent . '						We believe you will be satisfied by our services.';
                $buffer .= "\n";
                $buffer .= $indent . '					</div>';
                $buffer .= "\n";
                $buffer .= $indent . '				</div>';
                $buffer .= "\n";
                $buffer .= $indent . '				';
                $buffer .= "\n";
                $buffer .= $indent . '			</div>';
                $buffer .= "\n";
                $buffer .= $indent . '';
                $buffer .= "\n";
                $buffer .= $indent . '		</div></div>';
                $buffer .= "\n";
                $buffer .= $indent . '	 </div>';
                $buffer .= "\n";
                $buffer .= $indent . '	</div>';
                $buffer .= "\n";
                $buffer .= $indent . '</div>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }
}
